namespace HE.Investments.Loans.Common.Models.App;

public interface IAuth0Config
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? Domain { get; set; }

    public string? ManagementClientAudience { get; set; }

    public string? UserConnection { get; set; }
}
