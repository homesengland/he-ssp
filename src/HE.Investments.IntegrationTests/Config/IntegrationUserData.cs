using System.Globalization;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.Loans.IntegrationTests.Config;

public class IntegrationUserData : ILoginData
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

        FirstName = new FirstName("Integration")!;
        LastName = new LastName($"Test-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}")!;
    }

    public IntegrationUserData(UserConfig userConfig)
    {
        if (userConfig.UseConfigData)
        {
            UserGlobalId = userConfig.UserGlobalId;
            Email = userConfig.Email;
            FirstName = new FirstName(userConfig.FirstName)!;
            LastName = new LastName(userConfig.LastName)!;
            OrganizationName = userConfig.OrganizationName;
            OrganizationRegistrationNumber = userConfig.OrganizationRegistrationNumber;
            OrganizationAddress = userConfig.OrganizationAddress;
            TelephoneNumber = new TelephoneNumber(userConfig.TelephoneNumber);
            LoanApplicationIdInDraftState = userConfig.LoanApplicationIdInDraftState;
            ProjectInDraftStateId = userConfig.ProjectIdInDraftState;
            SubmittedLoanApplicationId = userConfig.SubmittedLoanApplicationId;

            IsDeveloperProvidedUserData = true;

            return;
        }

        FirstName = new FirstName("Integration")!;
        LastName = new LastName($"Test-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}")!;
    }

    public string UserGlobalId { get; private set; }

    public string Email { get; private set; }

    public string OrganizationName { get; private set; } = "HOMES OF ENGLAND LIMITED";

    public string OrganizationRegistrationNumber { get; private set; } = "02454505";

    public string OrganizationAddress { get; private set; } = "Heathers, Brushes Lane";

    public string LocalAuthorityId { get; private set; } = "E08000012";

    public string LocalAuthorityName { get; private set; } = "Liverpool";

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public JobTitle JobTitle { get; private set; }

    public string ContactName => $"{FirstName} {LastName}";

    public TelephoneNumber TelephoneNumber { get; private set; } = new("01427 611 833");

    public string LoanApplicationIdInDraftState { get; private set; }

    public string LoanApplicationName { get; private set; }

    public string SubmittedLoanApplicationId { get; private set; }

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

    public void SetLoanApplicationName()
    {
        if (LoanApplicationName.IsNotProvided())
        {
            LoanApplicationName = $"Application-{Guid.NewGuid()}";
        }
    }

    public void SetSubmittedLoanApplicationId(string loanApplicationId)
    {
        if (SubmittedLoanApplicationId.IsProvided() && IsDeveloperProvidedUserData)
        {
            return;
        }

        SubmittedLoanApplicationId = loanApplicationId;
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
        FirstName = new FirstName("John");
        LastName = new LastName("Doe");
        JobTitle = new JobTitle("Developer");
        TelephoneNumber = new TelephoneNumber("Carq pozdrawia");
        OrganizationName = "DO_NOT_DELETE_DEFAULT_ACCOUNT";
        OrganizationRegistrationNumber = "Not provided";
        OrganizationAddress = "12 Wharf Street";

        LoanApplicationIdInDraftState = string.Empty;
    }
}
