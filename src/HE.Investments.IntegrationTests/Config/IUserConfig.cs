namespace HE.InvestmentLoans.IntegrationTests.Config;

public interface IUserConfig
{
    string UserGlobalId { get; }

    string Email { get; }

    string OrganizationName { get; }

    string OrganizationRegistrationNumber { get; }

    string OrganizationAddress { get; }

    string ContactName { get; }

    string TelephoneNumber { get; }
}
