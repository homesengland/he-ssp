using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class ApplicationPagesUrls
{
    public const string TaskListSuffix = "/task-list";

    public const string CheckApplicationSuffix = "/check";

    public const string ApplicationSubmittedSuffix = "/submitted";

    public const string WithdrawSuffix = "/withdraw";

    public static string AboutLoanPage(string organisationId, FrontDoorProjectId? projectId) =>
        $"/loans/{organisationId}/application/about-loan".WithProject(projectId);

    public static string LoanApplyInformation(string organisationId, FrontDoorProjectId? projectId) =>
        $"/loans/{organisationId}/application/loan-apply-information".WithProject(projectId);

    public static string CheckYourDetails(string organisationId, FrontDoorProjectId? projectId) =>
        $"/loans/{organisationId}/application/check-your-details".WithProject(projectId);

    public static string LoanPurpose(string organisationId, FrontDoorProjectId? projectId) =>
        $"/loans/{organisationId}/application/loan-purpose".WithProject(projectId);

    public static string ApplicationName(string organisationId, FrontDoorProjectId? projectId = null) =>
        $"/loans/{organisationId}/application/application-name".WithProject(projectId);

    public static string TaskList(string organisationId, string applicationId) => $"{organisationId}/application/{applicationId}{TaskListSuffix}";

    public static string ApplicationDashboard(string organisationId, string applicationId) => $"{organisationId}/application/{applicationId}/dashboard";

    public static string ApplicationDashboardSupportingDocuments(string organisationId, string applicationId) =>
        $"{organisationId}/application/{applicationId}/dashboard/supporting-documents";

    private static string WithProject(this string url, FrontDoorProjectId? projectId)
    {
        return projectId.IsNotProvided() ? url : $"{url}?fdProjectId={projectId!.Value}";
    }
}
