using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.IntegrationTestsFramework.Data;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.Loans.IntegrationTests.Config;

public class IntegrationUserData
{
    public string LocalAuthorityName { get; private set; } = "Oxford";

    public string LoanApplicationIdInDraftState { get; private set; }

    public string LoanApplicationName { get; private set; }

    public string SubmittedLoanApplicationId { get; private set; }

    public string ProjectInDraftStateId { get; private set; }

    public IList<FileEntry> SupportingDocuments { get; private set; }

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
            new[] { SupportActivityType.DevelopingHomes });
    }

    public string GenerateApplicationName()
    {
        return LoanApplicationName = string.IsNullOrWhiteSpace(ProjectPrefillData?.Name)
            ? "IT-Application".WithTimestampSuffix()
            : ProjectPrefillData.Name.Replace("IT-FD-Project", "IT-Application");
    }

    public IList<FileEntry> GenerateSupportingDocuments()
    {
        SupportingDocuments = new List<FileEntry>()
        {
            new("document.pdf", "application/pdf", new MemoryStream(new byte[] { 1, 2, 3 })),
            new("another_documents.zip", "application/zip", new MemoryStream(new byte[] { 1, 2, 3 })),
        };
        return SupportingDocuments;
    }
}
