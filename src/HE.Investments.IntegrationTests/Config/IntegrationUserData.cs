using System.Globalization;

namespace HE.InvestmentLoans.IntegrationTests.Config;

public class IntegrationUserData
{
    public IntegrationUserData()
    {
        IsDeveloperProvidedUserData = false;
        UseUserWithAlreadyCompletedProfile();
        FirstName = "Integration";
        LastName = $"Test-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
    }

    public string UserGlobalId { get; private set; }

    public string Email { get; private set; }

    public string OrganizationName { get; private set; } = "HOMES OF ENGLAND LIMITED";

    public string OrganizationRegistrationNumber { get; private set; } = "02454505";

    public string OrganizationAddress { get; private set; } = "Heathers, Brushes Lane";

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string ContactName => $"{FirstName} {LastName}";

    public string TelephoneNumber { get; private set; } = "01427 611 833";

    public bool IsDeveloperProvidedUserData { get; }

    public void ProvideData(string userGlobalId)
    {
        if (IsDeveloperProvidedUserData)
        {
            throw new NotSupportedException("Developer provided user data and new user cannot be created");
        }

        UserGlobalId = userGlobalId;
        Email = $"{userGlobalId}@integrationTests.it";
    }

    public void UseUserWithAlreadyCompletedProfile()
    {
        if (IsDeveloperProvidedUserData is false)
        {
            return;
        }

        UserGlobalId = "real user global id";
        Email = "real email";
        FirstName = "real first name";
        LastName = "real last name";
        TelephoneNumber = "real telephone number";
        OrganizationName = "real organization name";
        OrganizationRegistrationNumber = "real organization registration number";
        OrganizationAddress = "real organization address";
    }
}
