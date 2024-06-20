using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.Loans.IntegrationTests.Config;

public class IntegrationUserData
{
    public string LocalAuthorityName { get; private set; } = "Oxford";

    public string LoanApplicationIdInDraftState { get; private set; }

    public string LoanApplicationName { get; private set; }

    public string SubmittedLoanApplicationId { get; private set; }

    public string ProjectInDraftStateId { get; private set; }

    public FrontDoorProjectData ProjectPrefillData { get; private set; }

    public string LocalAuthorityCode => "7000178";

    public void SetApplicationLoanId(string loanApplicationId)
    {
        LoanApplicationIdInDraftState = loanApplicationId;
    }

    public void SetSubmittedLoanApplicationId(string loanApplicationId)
    {
        SubmittedLoanApplicationId = loanApplicationId;
    }

    public void SetProjectId(string projectId)
    {
        ProjectInDraftStateId = projectId;
    }

    public FrontDoorProjectData GenerateProjectPrefillData()
    {
        return ProjectPrefillData = new FrontDoorProjectData(
            "IT-FD-Project".WithTimestampSuffix(),
            SupportActivityType.DevelopingHomes);
    }

    public string GenerateApplicationName()
    {
        return LoanApplicationName = string.IsNullOrWhiteSpace(ProjectPrefillData?.Name)
            ? "IT-Application".WithTimestampSuffix()
            : ProjectPrefillData.Name.Replace("IT-FD-Project", "IT-Application");
    }
}
