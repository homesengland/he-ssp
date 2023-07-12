namespace HE.InvestmentLoans.Common.Models.App;

public class DataverseConfig : IDataverseConfig
{
    public string? BaseUri { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }
}
