namespace HE.Investments.Loans.IntegrationTests.Config;

public class IntegrationUserData
{
    public string LocalAuthorityId { get; private set; } = "E08000012";

    public string LocalAuthorityName { get; private set; } = "Liverpool";

    public string LoanApplicationIdInDraftState { get; private set; }

    public string LoanApplicationName { get; private set; }

    public string SubmittedLoanApplicationId { get; private set; }

    public string ProjectInDraftStateId { get; private set; }

    public void SetApplicationLoanId(string loanApplicationId)
    {
        LoanApplicationIdInDraftState = loanApplicationId;
    }

    public void SetLoanApplicationName()
    {
        LoanApplicationName = $"Application-{Guid.NewGuid()}";
    }

    public void SetSubmittedLoanApplicationId(string loanApplicationId)
    {
        SubmittedLoanApplicationId = loanApplicationId;
    }

    public void SetProjectId(string projectId)
    {
        ProjectInDraftStateId = projectId;
    }
}
