namespace HE.Investments.Loans.Common.Models.App;

public interface IDataverseConfig
{
    public string? BaseUri { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }
}
