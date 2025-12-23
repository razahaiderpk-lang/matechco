using GeneralLedgerService.Domain;
using GeneralLedgerService.Data;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace GeneralLedgerService.Services;

public interface ILedgerService
{
    Task<JournalEntry> PostEntryAsync(JournalEntry entry);
    Task<decimal> GetAccountBalanceAsync(int accountId);
    Task<Dictionary<string, decimal>> GetTrialBalanceAsync();
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
        if (!entry.IsBalanced())
        {
            throw new InvalidOperationException("Journal entry is not balanced. Total Debits must equal Total Credits.");
        }

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
}
