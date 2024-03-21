using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.BusinessLogic.PrefillData.Crm;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.PrefillData.Repositories.LoanPrefillDataRepositoryTests;

public class GetLoanProjectPrefillDataTests : TestBase<LoanPrefillDataRepository>
{
    private static readonly FrontDoorProjectId ProjectId = new("test-project-123");

    private static readonly FrontDoorSiteId SiteId = new("test-site-123");

    private static readonly UserAccount UserAccount = UserAccountTestData.UserAccountOne;

    [Fact]
    public async Task ShouldThrowException_WhenLoanApplicationIsNotConnectedWithFrontDoorProject()
    {
        // given
        var applicationId = new LoanApplicationId(Guid.NewGuid());
        MockFrontDoorProjectId(applicationId, null);

        // when
        var getPrefillData = () => TestCandidate.GetLoanProjectPrefillData(applicationId, SiteId, UserAccount, CancellationToken.None);

        // then
        await getPrefillData.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ShouldReturnBasicPrefillData_WhenLoanApplicationIsConnectedWithFrontDoorProject()
    {
        // given
        var applicationId = new LoanApplicationId(Guid.NewGuid());
        MockFrontDoorProjectId(applicationId, ProjectId);
        MockPrefillDataRepository(siteName: "Site Name", 12);

        // when
        var result = await TestCandidate.GetLoanProjectPrefillData(applicationId, SiteId, UserAccount, CancellationToken.None);

        // then
        result.ProjectId.Should().Be(ProjectId);
        result.SiteId.Should().Be(SiteId);
        result.Name.Should().Be("Site Name");
        result.NumberOfHomes.Should().Be(12);
    }

    [Theory]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGranted, PlanningPermissionStatus.ReceivedFull)]
    [InlineData(SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice, PlanningPermissionStatus.NotSubmitted)]
    [InlineData(SitePlanningStatus.DetailedPlanningApplicationSubmitted, PlanningPermissionStatus.NotReceived)]
    [InlineData(SitePlanningStatus.OutlinePlanningApplicationSubmitted, PlanningPermissionStatus.NotReceived)]
    [InlineData(SitePlanningStatus.DetailedPlanningApprovalGrantedWithFurtherSteps, PlanningPermissionStatus.OutlineOrConsent)]
    [InlineData(SitePlanningStatus.OutlinePlanningApprovalGranted, PlanningPermissionStatus.OutlineOrConsent)]
    [InlineData(SitePlanningStatus.Undefined, null)]
    [InlineData(SitePlanningStatus.NoProgressOnPlanningApplication, null)]
    [InlineData(SitePlanningStatus.NoPlanningRequired, null)]
    public async Task ShouldPlanningPermissionStatus(SitePlanningStatus sitePlanningStatus, PlanningPermissionStatus? expectedStatus)
    {
        // given
        var applicationId = new LoanApplicationId(Guid.NewGuid());
        MockFrontDoorProjectId(applicationId, ProjectId);
        MockPrefillDataRepository(planningStatus: sitePlanningStatus);

        // when
        var result = await TestCandidate.GetLoanProjectPrefillData(applicationId, SiteId, UserAccount, CancellationToken.None);

        // then
        result.PlanningPermissionStatus.Should().Be(expectedStatus);
    }

    private void MockFrontDoorProjectId(LoanApplicationId applicationId, FrontDoorProjectId? projectId)
    {
        var crmContext = CreateAndRegisterDependencyMock<ILoanPrefillDataCrmContext>();

        crmContext.Setup(x => x.GetFrontDoorProjectId(applicationId, UserAccount, CancellationToken.None))
            .ReturnsAsync(projectId);
    }

    private void MockPrefillDataRepository(
        string? siteName = null,
        int? numberOfHomes = null,
        SitePlanningStatus planningStatus = SitePlanningStatus.Undefined)
    {
        var repository = CreateAndRegisterDependencyMock<IPrefillDataRepository>();

        repository.Setup(x => x.GetSitePrefillData(ProjectId, SiteId, CancellationToken.None))
            .ReturnsAsync(new SitePrefillData(
                SiteId,
                siteName ?? "Empty",
                numberOfHomes,
                planningStatus));
    }
}
