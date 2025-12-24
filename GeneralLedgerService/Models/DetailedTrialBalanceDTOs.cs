using System.Text.Json.Serialization;

namespace GeneralLedgerService.Models;

public class DetailedTrialBalanceResponse
{
    public bool Success { get; set; } = true;
    public DateOnly ReportDate { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string CompanyName { get; set; } = "Matecho Ledger Corp";
    public List<TrialBalanceAccountData> Data { get; set; } = new();
    public TrialBalanceSummary Summary { get; set; } = new();
    public TrialBalanceFormat Format { get; set; } = new();
}

public class TrialBalanceAccountData
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal DebitTotal { get; set; }
    public decimal CreditTotal { get; set; }
    public decimal NetBalance { get; set; }
    public string BalanceType { get; set; } = string.Empty; // "Debit" or "Credit"
}

public class TrialBalanceSummary
{
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public int DebitAccounts { get; set; }
    public int CreditAccounts { get; set; }
    public int TotalAccounts { get; set; }
    public string BalanceCheck { get; set; } = "Matched";
    public decimal Difference { get; set; }
}

public class TrialBalanceFormat
{
    public string Currency { get; set; } = "USD";
    public int DecimalPlaces { get; set; } = 2;
    public string NegativeFormat { get; set; } = "parentheses";
}
