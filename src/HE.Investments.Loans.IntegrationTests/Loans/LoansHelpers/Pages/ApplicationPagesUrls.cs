using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;

public static class ApplicationPagesUrls
{
    public const string TaskListSuffix = "/task-list";

    public const string CheckApplicationSuffix = "/check";

    public const string ApplicationSubmittedSuffix = "/submitted";

    public const string WithdrawSuffix = "/withdraw";

    public static string AboutLoanPage(FrontDoorProjectId? projectId) => "/loans/application/about-loan".WithProject(projectId);

    public static string LoanApplyInformation(FrontDoorProjectId? projectId) => "/loans/application/loan-apply-information".WithProject(projectId);

    public static string CheckYourDetails(FrontDoorProjectId? projectId) => "/loans/application/check-your-details".WithProject(projectId);

    public static string LoanPurpose(FrontDoorProjectId? projectId) => "/loans/application/loan-purpose".WithProject(projectId);

    public static string ApplicationName(FrontDoorProjectId? projectId = null) => "/loans/application/application-name".WithProject(projectId);

    public static string TaskList(string applicationId) => $"application/{applicationId}{TaskListSuffix}";

    public static string ApplicationDashboard(string applicationId) => $"application/{applicationId}/dashboard";

    private static string WithProject(this string url, FrontDoorProjectId? projectId)
    {
        return projectId.IsNotProvided() ? url : $"{url}?fdProjectId={projectId!.Value}";
    }
}
