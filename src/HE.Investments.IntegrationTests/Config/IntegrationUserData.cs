using System.Globalization;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.IntegrationTests.Config;

public class IntegrationUserData
{
    public IntegrationUserData()
    {
        // Change this flag to true when you want to use own user with already completed profile
        IsDeveloperProvidedUserData = false;
        if (IsDeveloperProvidedUserData)
        {
            UseDataProvidedByDeveloper();
            return;
        }

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

    public string LoanApplicationIdInDraftState { get; private set; }

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

    public void SetApplicationLoanId(string loanApplicationId)
    {
        if (LoanApplicationIdInDraftState.IsProvided())
        {
            return;
        }

        LoanApplicationIdInDraftState = loanApplicationId;
    }

    public void UseDataProvidedByDeveloper()
    {
        if (IsDeveloperProvidedUserData is false)
        {
            return;
        }

        UserGlobalId = "auth0|64a3bdb420d21a3fc5193e4d";
        Email = "luci_001@pwc.com";
        FirstName = "John";
        LastName = "Doe";
        TelephoneNumber = "Carq pozdrawia";
        OrganizationName = "DO_NOT_DELETE_DEFAULT_ACCOUNT";
        OrganizationRegistrationNumber = "Not provided";
        OrganizationAddress = "12 Wharf Street";

        LoanApplicationIdInDraftState = "6ef83a12-4659-ee11-be6f-002248c653e1";
    }
}
