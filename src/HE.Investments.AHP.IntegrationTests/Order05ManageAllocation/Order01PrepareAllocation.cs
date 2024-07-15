using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.AllocationClaims.Const;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.HomeTypes;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order05ManageAllocation;

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
    }

    public SchemeInformationData SchemeInformationData { get; }

    public FinancialDetailsData FinancialDetailsData { get; }

    public HomeTypesData HomeTypesData { get; }

    public DeliveryPhasesData DeliveryPhasesData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_AhpProjectShouldBeCreated()
    {
        // given
        var allocationId = await AhpDataManipulator.CreateAhpAllocation(
            LoginData,
            ApplicationData,
            FinancialDetailsData,
            SchemeInformationData,
            HomeTypesData,
            DeliveryPhasesData);
        AllocationData.SetAllocationId(allocationId);

        // when
        var currentPage = await TestClient.NavigateTo(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, allocationId));

        // then
        currentPage
            .UrlEndWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, allocationId))
            .HasTitle(ClaimPageTitles.Summary);
    }
}
