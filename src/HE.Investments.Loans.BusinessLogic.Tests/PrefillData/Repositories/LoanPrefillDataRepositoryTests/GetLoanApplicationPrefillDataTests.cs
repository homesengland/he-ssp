using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.PrefillData.Repositories.LoanPrefillDataRepositoryTests;

public class GetLoanApplicationPrefillDataTests : TestBase<LoanPrefillDataRepository>
{
    private static readonly FrontDoorProjectId ProjectId = new("test-123");

    private static readonly UserAccount UserAccount = UserAccountTestData.UserAccountOne;

    [Fact]
    public async Task ShouldReturnBasicPrefillData()
    {
        // given
        MockPrefillDataRepository(projectName: "Some Project Name", siteId: "test-site-123");

        // when
        var result = await TestCandidate.GetLoanApplicationPrefillData(ProjectId, UserAccount, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.ProjectId.Should().Be(ProjectId);
        result.Name.Should().Be("Some Project Name");
        result.SiteId.Should().NotBeNull();
        result.SiteId!.Value.Should().Be("test-site-123");
    }

    [Fact]
    public async Task ShouldReturnBuildingNewHomesFundingPurpose_WhenActivityTypeIsDevelopingHomes()
    {
        // given
        MockPrefillDataRepository(activityTypes: new[] { SupportActivityType.DevelopingHomes });

        // when
        var result = await TestCandidate.GetLoanApplicationPrefillData(ProjectId, UserAccount, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.FundingPurpose.Should().Be(FundingPurpose.BuildingNewHomes);
    }

    [Fact]
    public async Task ShouldReturnNullFundingPurpose_WhenThereAreMultipleActivityTypes()
    {
        // given
        MockPrefillDataRepository(activityTypes: new[] { SupportActivityType.DevelopingHomes, SupportActivityType.ProvidingInfrastructure });

        // when
        var result = await TestCandidate.GetLoanApplicationPrefillData(ProjectId, UserAccount, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.FundingPurpose.Should().BeNull();
    }

    [Theory]
    [InlineData(SupportActivityType.Other)]
    [InlineData(SupportActivityType.AcquiringLand)]
    [InlineData(SupportActivityType.ProvidingInfrastructure)]
    [InlineData(SupportActivityType.SellingLandToHomesEngland)]
    [InlineData(SupportActivityType.ManufacturingHomesWithinFactory)]
    public async Task ShouldReturnNullFundingPurpose_WhenActivityTypeIs(SupportActivityType activityType)
    {
        // given
        MockPrefillDataRepository(activityTypes: new[] { activityType });

        // when
        var result = await TestCandidate.GetLoanApplicationPrefillData(ProjectId, UserAccount, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.FundingPurpose.Should().BeNull();
    }

    private void MockPrefillDataRepository(
        string? projectName = null,
        IEnumerable<SupportActivityType>? activityTypes = null,
        string? siteId = null)
    {
        var repository = CreateAndRegisterDependencyMock<IPrefillDataRepository>();

        repository.Setup(x => x.GetProjectPrefillData(ProjectId, UserAccount, CancellationToken.None))
            .ReturnsAsync(new ProjectPrefillData(
                ProjectId,
                projectName ?? "Empty",
                activityTypes?.ToList() ?? new List<SupportActivityType>(),
                siteId != null ? new FrontDoorSiteId(siteId) : null));
    }
}
