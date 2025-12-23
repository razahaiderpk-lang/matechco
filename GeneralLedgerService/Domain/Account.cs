namespace GeneralLedgerService.Domain;

public enum AccountType
{
    Asset=1,
    Liability=2,
    Equity=3,
    Revenue=4,
    Expense=5
}

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}
