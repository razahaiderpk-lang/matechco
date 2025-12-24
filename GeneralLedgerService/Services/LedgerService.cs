using GeneralLedgerService.Domain;
using GeneralLedgerService.Data;
using GeneralLedgerService.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace GeneralLedgerService.Services;

public interface ILedgerService
{
    Task<JournalEntry> PostEntryAsync(JournalEntry entry);
    Task<decimal> GetAccountBalanceAsync(int accountId);
    Task<Dictionary<string, decimal>> GetTrialBalanceAsync();
    Task<BalanceSheetResponse> GetBalanceSheetAsync(DateOnly asOfDate);
}

public class LedgerService : ILedgerService
{
    private readonly LedgerDbContext _context;

    public LedgerService(LedgerDbContext context)
    {
        _context = context;
    }

    public async Task<JournalEntry> PostEntryAsync(JournalEntry entry)
    {
        entry.Validate();

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.JournalEntries.Add(entry);

            foreach (var line in entry.Lines)
            {
                var account = await _context.Accounts.FindAsync(line.AccountId);
                if (account == null)
                {
                    throw new KeyNotFoundException($"Account with ID {line.AccountId} not found.");
                }

                // Update account balance
                // Rules: Debit increases Assets/Expenses, decreases Liabilities/Equity/Revenue
                // Here we use a simpler model where Amount is already signed (+ for debit, - for credit or vice versa)
                // Let's assume + is Debit, - is Credit.
                account.Balance += line.Amount;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return entry;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        return account?.Balance ?? 0;
    }

    public async Task<Dictionary<string, decimal>> GetTrialBalanceAsync()
    {
        return await _context.Accounts
            .ToDictionaryAsync(a => a.Name, a => a.Balance);
    }

    public async Task<BalanceSheetResponse> GetBalanceSheetAsync(DateOnly asOfDate)
    {
        // Calculate balances "as of" date by summing all journal entries up to that date
        var balances = await _context.JournalEntryLines
            .Include(l => l.JournalEntry)
            .Where(l => l.JournalEntry.Date <= asOfDate)
            .GroupBy(l => l.AccountId)
            .Select(g => new
            {
                AccountId = g.Key,
                Balance = g.Sum(l => l.Amount)
            })
            .ToListAsync();

        var accounts = await _context.Accounts.ToListAsync();

        // Join calculated balances with account definitions
        var accountBalances = from account in accounts
                              join balance in balances on account.Id equals balance.AccountId into joined
                              from b in joined.DefaultIfEmpty()
                              select new
                              {
                                  account.Name,
                                  account.Type,
                                  Balance = b?.Balance ?? 0
                              };

        var assetAccounts = accountBalances.Where(a => a.Type == AccountType.Asset).ToList();
        var liabilityAccounts = accountBalances.Where(a => a.Type == AccountType.Liability).ToList();
        var equityAccounts = accountBalances.Where(a => a.Type == AccountType.Equity).ToList();
        
        // Calculate Net Income (Revenue + Expenses, where expenses are negative)
        // Note: In accounting, Net Income flows into Retained Earnings (Equity)
        var revenueTotal = accountBalances.Where(a => a.Type == AccountType.Revenue).Sum(a => a.Balance);
        var expenseTotal = accountBalances.Where(a => a.Type == AccountType.Expense).Sum(a => a.Balance);
        var netIncome = revenueTotal + expenseTotal;

        var response = new BalanceSheetResponse
        {
            Asset = new AssetSection
            {
                Ledgers = assetAccounts.Select(a => new Dictionary<string, decimal> { { a.Name, a.Balance } }).ToList(),
                TotalAsset = assetAccounts.Sum(a => a.Balance)
            },
            LiabilityEquity = new LiabilityEquitySection
            {
                Liability = new LiabilitySubsection
                {
                    Ledgers = liabilityAccounts.Select(a => new Dictionary<string, decimal> { { a.Name, a.Balance } }).ToList(),
                    TotalLiability = liabilityAccounts.Sum(a => a.Balance)
                },
                Equity = new EquitySubsection
                {
                    Ledgers = equityAccounts.Select(a => new Dictionary<string, decimal> { { a.Name, a.Balance } }).ToList(),
                    TotalEquity = equityAccounts.Sum(a => a.Balance)
                },
                TotalLiabilityEquity = liabilityAccounts.Sum(a => a.Balance) + equityAccounts.Sum(a => a.Balance) + netIncome
            }
        };

        // Add Net Income as a line item in Equity to balance the report
        response.LiabilityEquity.Equity.Ledgers.Add(new Dictionary<string, decimal> { { "Net Income (Loss)", netIncome } });
        response.LiabilityEquity.Equity.TotalEquity += netIncome;

        return response;
    }
}
