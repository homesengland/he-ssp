namespace HE.Investments.Loans.IntegrationTests.Config;
public class UserConfig
{
    public bool UseConfigData { get; set; }

    public string UserGlobalId { get; set; }

    public string Email { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationRegistrationNumber { get; set; }

    public string OrganizationAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string TelephoneNumber { get; set; }

    public string LoanApplicationIdInDraftState { get; set; }

    public string SubmittedLoanApplicationId { get; private set; }

    public string ProjectIdInDraftState { get; set; }
}
