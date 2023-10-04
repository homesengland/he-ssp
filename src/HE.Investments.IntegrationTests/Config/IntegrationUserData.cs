using System.Globalization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.User.ValueObjects;

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

        FirstName = FirstName.FromString("Integration");
        LastName = LastName.FromString($"Test-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
    }

    public IntegrationUserData(UserConfig userConfig)
    {
        if (userConfig.UseConfigData)
        {
            UserGlobalId = userConfig.UserGlobalId;
            Email = userConfig.Email;
            FirstName = FirstName.FromString(userConfig.FirstName);
            LastName = LastName.FromString(userConfig.LastName);
            OrganizationName = userConfig.OrganizationName;
            OrganizationRegistrationNumber = userConfig.OrganizationRegistrationNumber;
            OrganizationAddress = userConfig.OrganizationAddress;
            TelephoneNumber = TelephoneNumber.FromString(userConfig.TelephoneNumber);
            LoanApplicationIdInDraftState = userConfig.LoanApplicationIdInDraftState;
            ProjectInDraftStateId = userConfig.ProjectIdInDraftState;

            IsDeveloperProvidedUserData = true;

            return;
        }

        FirstName = FirstName.FromString("Integration");
        LastName = LastName.FromString($"Test-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
    }

    public string UserGlobalId { get; private set; }

    public string Email { get; private set; }

    public string OrganizationName { get; private set; } = "HOMES OF ENGLAND LIMITED";

    public string OrganizationRegistrationNumber { get; private set; } = "02454505";

    public string OrganizationAddress { get; private set; } = "Heathers, Brushes Lane";

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public JobTitle JobTitle { get; private set; }

    public string ContactName => $"{FirstName} {LastName}";

    public TelephoneNumber TelephoneNumber { get; private set; } = TelephoneNumber.FromString("01427 611 833");

    public string LoanApplicationIdInDraftState { get; private set; }

    public string ProjectInDraftStateId { get; private set; }

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
        if (LoanApplicationIdInDraftState.IsProvided() && IsDeveloperProvidedUserData)
        {
            return;
        }

        LoanApplicationIdInDraftState = loanApplicationId;
    }

    public void SetProjectId(string projectId)
    {
        if (ProjectInDraftStateId.IsProvided() && IsDeveloperProvidedUserData)
        {
            return;
        }

        ProjectInDraftStateId = projectId;
    }

    public void UseDataProvidedByDeveloper()
    {
        if (IsDeveloperProvidedUserData is false)
        {
            return;
        }

        UserGlobalId = "auth0|64a3bdb420d21a3fc5193e4d";
        Email = "luci_001@pwc.com";
        FirstName = FirstName.FromString("John");
        LastName = LastName.FromString("Doe");
        JobTitle = JobTitle.FromString("Developer");
        TelephoneNumber = TelephoneNumber.FromString("Carq pozdrawia");
        OrganizationName = "DO_NOT_DELETE_DEFAULT_ACCOUNT";
        OrganizationRegistrationNumber = "Not provided";
        OrganizationAddress = "12 Wharf Street";

        LoanApplicationIdInDraftState = "6ef83a12-4659-ee11-be6f-002248c653e1";
    }
}
