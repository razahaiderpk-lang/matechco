using System.Text.Json.Serialization;

namespace GeneralLedgerService.Models;

public class AdvancedTrialBalanceResponse
{
    public AssetSection Asset { get; set; } = new();

    [JsonPropertyName("Liability & Equity")]
    public LiabilityEquitySection LiabilityEquity { get; set; } = new();
}

public class AssetSection
{
    [JsonPropertyName("ledgers")]
    public List<Dictionary<string, decimal>> Ledgers { get; set; } = new();

    [JsonPropertyName("Total Asset")]
    public decimal TotalAsset { get; set; }
}

public class LiabilityEquitySection
{
    public LiabilitySubsection Liability { get; set; } = new();
    public EquitySubsection Equity { get; set; } = new();

    [JsonPropertyName("Total Liability & Equity")]
    public decimal TotalLiabilityEquity { get; set; }
}

public class LiabilitySubsection
{
    [JsonPropertyName("ledgers")]
    public List<Dictionary<string, decimal>> Ledgers { get; set; } = new();

    [JsonPropertyName("Total Liability")]
    public decimal TotalLiability { get; set; }
}

public class EquitySubsection
{
    [JsonPropertyName("ledgers")]
    public List<Dictionary<string, decimal>> Ledgers { get; set; } = new();

    [JsonPropertyName("Total Equity")]
    public decimal TotalEquity { get; set; }
}
