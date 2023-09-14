namespace HE.InvestmentLoans.IntegrationTests.Config;

public class UserConfig
{
    public UserConfig(string userGlobalId, string email, string organizationName, string organizationRegistrationNumber, string organizationAddress, string contactName, string telephoneNumer)
    {
        UserGlobalId = userGlobalId;
        Email = email;
        OrganizationName = organizationName;
        OrganizationRegistrationNumber = organizationRegistrationNumber;
        OrganizationAddress = organizationAddress;
        ContactName = contactName;
        TelephoneNumer = telephoneNumer;
    }

    public UserConfig()
    {
    }

    public string UserGlobalId = "auth0|649c218847ce4d2aed96b72a";

    public string Email = "luci_001@pwc.com";

    public string OrganizationName = "DO_NOT_DELETE_DEFAULT_ACCOUNT";

    public string OrganizationRegistrationNumber = "Not provided";

    public string OrganizationAddress = "12 Wharf Street";

    public string ContactName = "John Doe";

    public string TelephoneNumer = "Carq pozdrawia";

}
