using System.Text.Json.Serialization;

namespace GeneralLedgerService.Models;

public class AccountLedgerResponse
{
    public bool Success { get; set; } = true;
    public AccountLedgerHeader Account { get; set; } = new();
    public LedgerPeriod Period { get; set; } = new();
    public OpeningClosingBalance OpeningBalance { get; set; } = new();
    public List<LedgerTransaction> Transactions { get; set; } = new();
    public OpeningClosingBalance ClosingBalance { get; set; } = new();
    public LedgerSummary Summary { get; set; } = new();
    public LedgerPagination Pagination { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public LedgerFormat Format { get; set; } = new();
}

public class AccountLedgerHeader
{
    public string AccountId { get; set; } = string.Empty;
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string NormalBalance { get; set; } = string.Empty;
    public string Description { get; set; } = "N/A";
    public string Category { get; set; } = "N/A";
    public string Status { get; set; } = "Active";
}

public class LedgerPeriod
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string FinancialYear { get; set; } = "N/A";
}

public class OpeningClosingBalance
{
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "Debit" or "Credit"
}

public class LedgerTransaction
{
    public string TransactionId { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string VoucherNumber { get; set; } = "N/A";
    public string VoucherType { get; set; } = "Journal";
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public string BalanceType { get; set; } = string.Empty;
    public Counterparty? Counterparty { get; set; }
    public string? InvoiceNumber { get; set; }
    public string PostedBy { get; set; } = "System";
    public DateTime PostedAt { get; set; }
}

public class Counterparty
{
    public string? Name { get; set; }
    public string? Id { get; set; }
}

public class LedgerSummary
{
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal NetMovement { get; set; }
    public int TransactionCount { get; set; }
    public decimal AverageDebit { get; set; }
    public decimal AverageCredit { get; set; }
    public decimal HighestTransaction { get; set; }
    public decimal LowestTransaction { get; set; }
}

public class LedgerPagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}

public class LedgerFormat
{
    public string Currency { get; set; } = "USD";
    public int DecimalPlaces { get; set; } = 2;
}
