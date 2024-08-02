using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.AllocationClaims.Const;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.HomeTypes;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Phase;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(501)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01PrepareAllocation : AhpIntegrationTest
{
    public Order01PrepareAllocation(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        SchemeInformationData = ReturnSharedData<SchemeInformationData>();
        FinancialDetailsData = ReturnSharedData<FinancialDetailsData>(data =>
        {
            var schemeInformationData = GetSharedDataOrNull<SchemeInformationData>(nameof(SchemeInformationData));
            data.ProvideSchemeFunding(schemeInformationData?.RequiredFunding ?? 0m);
        });
        HomeTypesData = ReturnSharedData<HomeTypesData>();
        DeliveryPhasesData = ReturnSharedData<DeliveryPhasesData>();
        PhaseData = ReturnSharedData<PhaseData>();
    }

    public SchemeInformationData SchemeInformationData { get; }

    public FinancialDetailsData FinancialDetailsData { get; }

    public HomeTypesData HomeTypesData { get; }

    public DeliveryPhasesData DeliveryPhasesData { get; }

    public PhaseData PhaseData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_AhpAllocationShouldBeCreated()
    {
        // given
        AllocationData.GenerateAllocationName();
        SchemeInformationData.PopulateAllData();
        FinancialDetailsData.PopulateAllData(SchemeInformationData.RequiredFunding);
        HomeTypesData.General.PopulateAllData();
        HomeTypesData.Disabled.PopulateAllData();
        DeliveryPhasesData.RehabDeliveryPhase.PopulateAllData();
        DeliveryPhasesData.OffTheShelfDeliveryPhase.PopulateAllData();

        await AhpProjectShouldExist();

        var allocationId = await AhpDataManipulator.CreateAhpAllocation(
            LoginData,
            SiteData,
            FinancialDetailsData,
            SchemeInformationData,
            HomeTypesData,
            DeliveryPhasesData,
            AllocationData);
        AllocationData.SetAllocationId(allocationId);
        AllocationData.SetFromApplicationData(ApplicationData, SchemeInformationData.RequiredFunding);
        PhaseData.SetPhaseId(DeliveryPhasesData.RehabDeliveryPhase.Id);
        PhaseData.SetDataFromDeliveryPhase(DeliveryPhasesData.RehabDeliveryPhase, SchemeInformationData.RequiredFunding, SchemeInformationData.HousesToDeliver / 2);

        // when
        var currentPage = await TestClient.NavigateTo(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, allocationId));

        // then
        currentPage
            .UrlEndWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, allocationId))
            .HasTitle(ClaimPageTitles.Summary);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_DisplayAllocationOnProjectDashboard()
    {
        // given
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));
        currentPage
            .HasTitle(ProjectData.ProjectName)
            .UrlEndWith(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));

        // when
        var projectCard = currentPage.GetFirstListCard();

        // then
        var allocationsCard
            = projectCard.ContentList.SingleOrDefault(x => x.Title == "Allocations");
        allocationsCard.Should().NotBeNull("Allocations should be displayed on the project dashboard");
        allocationsCard!.Description.Should().NotBeNullOrEmpty("Allocations description should be displayed");
        var allocation = allocationsCard.Items.SingleOrDefault(x => x.Name == AllocationData.AllocationName);
        allocation.Should().NotBeNull("New allocation should be displayed on the project dashboard");
    }

    private async Task AhpProjectShouldExist()
    {
        if (ProjectData.IsProjectNotCreated())
        {
            var (projectId, siteId) =
                await InFrontDoor.FrontDoorProjectEligibleForAhpExist(
                    LoginData,
                    ProjectData.GenerateProjectName("Only for Allocation"),
                    SiteData.GenerateSiteName());

            ProjectData.SetProjectId(projectId);
            var consortiumId = await AhpCrmContext.GetAhpConsortium(LoginData);
            await AhpProjectDataManipulator.CreateAhpProject(LoginData, ProjectData.ProjectId, ProjectData.ProjectName, siteId, SiteData.SiteName, consortiumId?.Id?.Value);
            var ahpProject = await AhpProjectDataManipulator.GetAhpProject(LoginData, projectId);
            SiteData.SetSiteId(ahpProject.ListOfSites.Single().id);
            await AhpDataManipulator.MakeSiteUsableForAllocation(LoginData, SiteData);
        }
    }
}
