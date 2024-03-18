using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.Loans.IntegrationTests.Config;

public class IntegrationUserData
{
    public string LocalAuthorityId { get; private set; } = "E08000012";

    public string LocalAuthorityName { get; private set; } = "Liverpool";

    public string LoanApplicationIdInDraftState { get; private set; }

    public string LoanApplicationName { get; private set; }

    public string SubmittedLoanApplicationId { get; private set; }

    public string ProjectInDraftStateId { get; private set; }

    public ProjectPrefillData ProjectPrefillData { get; private set; }

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

    public ProjectPrefillData GenerateProjectPrefillData()
    {
        return ProjectPrefillData = new ProjectPrefillData(
            new FrontDoorProjectId(Guid.NewGuid().ToString()),
            true,
            "IT-FD-Project".WithTimestampSuffix(),
            new[] { SupportActivityType.DevelopingHomes },
            AffordableHomesAmount.OnlyAffordableHomes,
            10,
            Array.Empty<InfrastructureType>(),
            false,
            new SiteNotIdentifiedDetails(ProjectGeographicFocus.National, null, 20),
            true,
            true,
            new FundingDetails(RequiredFundingOption.Between5MlnAnd10Mln, true),
            new DateDetails("01", "12", "2025"));
    }

    public string GenerateApplicationName()
    {
        return LoanApplicationName = string.IsNullOrWhiteSpace(ProjectPrefillData?.Name)
            ? "IT-Application".WithTimestampSuffix()
            : ProjectPrefillData.Name.Replace("IT-FD-Project", "IT-Application");
    }
}
