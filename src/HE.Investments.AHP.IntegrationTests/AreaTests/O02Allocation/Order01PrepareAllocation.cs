using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.HomeTypes;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Phase;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Extensions;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(501)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01PrepareAllocation : AhpIntegrationTest
{
    private readonly ITestOutputHelper _output;

    public Order01PrepareAllocation(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        _output = output;
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
        await AhpProjectShouldExist();
        await AhpApprovedApplicationShouldExist();
        var elapsed = await WaitFor(
            AhpAllocationExistsOnProjectDetailsPage,
            timeout: TimeSpan.FromSeconds(45),
            refreshDelay: TimeSpan.FromSeconds(6));

        _output.WriteLine($"Allocation created within: {elapsed.TotalSeconds} sec");
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_DisplayAllocationOnAllocationListView()
    {
        // given
        var projectDetailsPage = await GetCurrentPage(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));
        projectDetailsPage
            .HasTitle(ProjectData.ProjectName)
            .UrlEndWith(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));

        // when
        var allocationsListPage = await TestClient.NavigateTo(projectDetailsPage.GetLinkByTestId("view-all-Allocations"));

        // then
        allocationsListPage
            .UrlEndWith(AllocationPagesUrl.ProjectAllocationList(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value))
            .HasTitle("Affordable Homes Programme 2021-2026 Continuous Market Engagement allocations")
            .HasPageHeader(header: "Active allocations")
            .HasTableRowsHeaders(
            [
                "Name",
                "Homes",
                "Tenure",
                "Local authority",
            ])
            .HasLinkWithText(AllocationData.AllocationName, out _)
            .HasTableRowWithValues(
            [
                SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture),
                AllocationData.Tenure.GetDescription(),
                SiteData.LocalAuthorityName,
            ]);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_DisplayManageAllocation()
    {
        // given
        var allocationsListPage = await GetCurrentPage(AllocationPagesUrl.ProjectAllocationList(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value));
        allocationsListPage
            .UrlEndWith(AllocationPagesUrl.ProjectAllocationList(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value))
            .HasTitle("Affordable Homes Programme 2021-2026 Continuous Market Engagement allocations")
            .HasLinkWithText(AllocationData.AllocationName, out var manageAllocationLink);

        // when
        var manageAllocationPage = await TestClient.NavigateTo(manageAllocationLink);

        // then
        manageAllocationPage
            .UrlEndWith(AllocationPagesUrl.ManageAllocation(UserOrganisationData.OrganisationId, AllocationData.AllocationId))
            .HasTitle(AllocationData.AllocationName)
            .HasLinkWithText("Manage claims", out _);
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
            await AhpProjectDataManipulator.CreateAhpProject(LoginData, ProjectData.ProjectId, ProjectData.ProjectName, siteId, SiteData.SiteName, consortiumId?.Id.Value);
            var ahpProject = await AhpProjectDataManipulator.GetAhpProject(LoginData, projectId);
            SiteData.SetSiteId(ahpProject.ListOfSites.Single().id);
            await AhpDataManipulator.MakeSiteUsableForAllocation(LoginData, SiteData);
        }
    }

    private async Task AhpApprovedApplicationShouldExist()
    {
        AllocationData.GenerateAllocationName();
        SchemeInformationData.PopulateAllData();
        FinancialDetailsData.PopulateAllData(SchemeInformationData.RequiredFunding);
        HomeTypesData.General.PopulateAllData();
        HomeTypesData.Disabled.PopulateAllData();
        DeliveryPhasesData.RehabDeliveryPhase.PopulateAllData();
        DeliveryPhasesData.OffTheShelfDeliveryPhase.PopulateAllData();

        var applicationId = await AhpDataManipulator.CreateAhpApplication(
            LoginData,
            SiteData,
            FinancialDetailsData,
            SchemeInformationData,
            HomeTypesData,
            DeliveryPhasesData,
            AllocationData);
        AllocationData.SetFromApplicationData(ApplicationData, SchemeInformationData.RequiredFunding);
        PhaseData.SetDataFromDeliveryPhase(DeliveryPhasesData.RehabDeliveryPhase, SchemeInformationData.RequiredFunding, SchemeInformationData.HousesToDeliver / 2);

        await ChangeApplicationStatus(applicationId, ApplicationStatus.Approved);
    }

    private async Task<bool> AhpAllocationExistsOnProjectDetailsPage()
    {
        var projectDetailsPage = await TestClient.NavigateTo(
            ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId));
        projectDetailsPage
            .HasTitle(ProjectData.ProjectName)
            .UrlEndWith(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ProjectData.ProjectId))
            .HasSummaryCardSection("Allocations")
            .HasLinkWithText(AllocationData.AllocationName, out var allocationLink); // search for link globally when Application is not returned

        var allocationId = allocationLink.ExtractParameter("allocationId", AllocationPagesUrl.ManageAllocation(string.Empty, "{allocationId}"));
        AllocationData.SetAllocationId(allocationId);

        return true;
    }
}
