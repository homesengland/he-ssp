namespace HE.InvestmentLoans.IntegrationTests.Config;

public class UserConfig : IUserConfig
{
    public string UserGlobalId { get; set; }

    public string Email { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationRegistrationNumber { get; set; }

    public string OrganizationAddress { get; set; }

    public string ContactName { get; set; }

    public string TelephoneNumber { get; set; }
}
