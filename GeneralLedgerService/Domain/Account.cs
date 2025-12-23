namespace GeneralLedgerService.Domain;

public enum AccountType
{
    Asset,
    Liability,
    Equity,
    Revenue,
    Expense
}

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}
