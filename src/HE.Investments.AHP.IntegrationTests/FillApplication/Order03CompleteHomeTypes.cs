using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.HomeTypes.Const;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03CompleteHomeTypes : AhpIntegrationTest
{
    private readonly HomeTypesData _homeTypesData;

    public Order03CompleteHomeTypes(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        var homeTypesData = GetSharedDataOrNull<HomeTypesData>(nameof(_homeTypesData));
        if (homeTypesData is null)
        {
            homeTypesData = new HomeTypesData();
            SetSharedData(nameof(_homeTypesData), homeTypesData);
        }

        _homeTypesData = homeTypesData;
    }

    private GeneralHomeTypeData GeneralHomeType => _homeTypesData.General;

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_HomeTypesLandingPage()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithId("add-home-type", out var enterHomeTypesSection);

        // when
        var landingPage = await TestClient.NavigateTo(enterHomeTypesSection);

        // then
        landingPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.LandingPage))
            .HasTitle(HomeTypesPageTitles.LandingPage)
            .HasGdsLinkButton("continue-button", out var continueButton);

        (await TestClient.NavigateTo(continueButton)).UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List));
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_AddGeneralHomeType()
    {
        // given
        var homeTypeListPage = await TestClient.NavigateTo(HomeTypesPagesUrl.List(ApplicationData.ApplicationId));
        homeTypeListPage.UrlEndWith(HomeTypesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasLinkWithId("add-home-type", out var enterNewHomeTypePage);

        // when
        var newHomeTypePage = await TestClient.NavigateTo(enterNewHomeTypePage);

        // then
        newHomeTypePage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.NewHomeType))
            .HasTitle(HomeTypesPageTitles.HomeTypeDetails)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        var homeType = GeneralHomeType.GenerateHomeTypeDetails();
        var homeInformationPage = await TestClient.SubmitButton(
            continueButton,
            ("HomeTypeName", homeType.Name),
            ("HousingType", homeType.HousingType.ToString()));

        GeneralHomeType.SetHomeTypeId(homeInformationPage.Url.GetHomeTypeGuidFromUrl());
        homeInformationPage.UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.HomeInformation, GeneralHomeType));

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order04_ProvideHomeInformation()
    {
        // given
        var homeType = GeneralHomeType.GenerateInformation();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.HomeInformation, GeneralHomeType),
            HomeTypesPageTitles.HomeInformation,
            BuildHomeTypePage(HomeTypePagesUrl.MoveOnAccommodation, GeneralHomeType),
            ("NumberOfHomes", homeType.NumberOfHomes.ToString(CultureInfo.InvariantCulture)),
            ("NumberOfBedrooms", homeType.NumberOfBedrooms.ToString(CultureInfo.InvariantCulture)),
            ("MaximumOccupancy", homeType.MaximumOccupancy.ToString(CultureInfo.InvariantCulture)),
            ("NumberOfStoreys", homeType.NumberOfStoreys.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideMoveOnAccommodation()
    {
        // given
        var homeType = GeneralHomeType.GenerateMoveOnAccommodation();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.MoveOnAccommodation, GeneralHomeType),
            HomeTypesPageTitles.MoveOnAccommodation,
            BuildHomeTypePage(HomeTypePagesUrl.BuildingInformation, GeneralHomeType),
            ("IntendedAsMoveOnAccommodation", homeType.MoveOnAccommodation.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideBuildingInformation()
    {
        // given
        var homeType = GeneralHomeType.GenerateBuildingInformation();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.BuildingInformation, GeneralHomeType),
            HomeTypesPageTitles.BuildingInformation,
            BuildHomeTypePage(HomeTypePagesUrl.CustomBuildProperty, GeneralHomeType),
            ("BuildingType", homeType.BuildingType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideCustomBuildProperty()
    {
        // given
        var homeType = GeneralHomeType.GenerateCustomBuild();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.CustomBuildProperty, GeneralHomeType),
            HomeTypesPageTitles.CustomBuildProperty,
            BuildHomeTypePage(HomeTypePagesUrl.TypeOfFacilities, GeneralHomeType),
            ("CustomBuild", homeType.CustomBuild.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ProvideFacilityType()
    {
        // given
        var homeType = GeneralHomeType.GenerateFacilityType();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.TypeOfFacilities, GeneralHomeType),
            HomeTypesPageTitles.TypeOfFacilities,
            BuildHomeTypePage(HomeTypePagesUrl.AccessibilityStandards, GeneralHomeType),
            ("FacilityType", homeType.FacilityType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ProvideAccessibilityStandards()
    {
        // given
        var homeType = GeneralHomeType.GenerateAccessibilityStandards();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.AccessibilityStandards, GeneralHomeType),
            HomeTypesPageTitles.AccessibilityStandards,
            BuildHomeTypePage(HomeTypePagesUrl.AccessibilityCategory, GeneralHomeType),
            ("AccessibilityStandards", homeType.AccessibilityStandards.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ProvideAccessibilityCategory()
    {
        // given
        var homeType = GeneralHomeType.GenerateAccessibilityCategory();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.AccessibilityCategory, GeneralHomeType),
            HomeTypesPageTitles.AccessibilityCategory,
            BuildHomeTypePage(HomeTypePagesUrl.FloorArea, GeneralHomeType),
            ("AccessibilityCategory", homeType.AccessibilityCategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ProvideFloorArea()
    {
        // given
        var homeType = GeneralHomeType.GenerateFloorArea();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.FloorArea, GeneralHomeType),
            HomeTypesPageTitles.FloorArea,
            BuildHomeTypePage(HomeTypePagesUrl.FloorAreaStandards, GeneralHomeType),
            ("FloorArea", homeType.FloorArea.ToString(CultureInfo.InvariantCulture)),
            ("MeetNationallyDescribedSpaceStandards", homeType.MeetSpaceStandards.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ProvideFloorAreaStandards()
    {
        // given
        var homeType = GeneralHomeType.GenerateFloorAreaStandards();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.FloorAreaStandards, GeneralHomeType),
            HomeTypesPageTitles.FloorAreaStandards,
            BuildHomeTypePage(HomeTypePagesUrl.AffordableRent, GeneralHomeType),
            ("NationallyDescribedSpaceStandards", homeType.SpaceStandards.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ProvideAffordableRent()
    {
        // given
        var homeType = GeneralHomeType.GenerateAffordableRent();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.AffordableRent, GeneralHomeType),
            HomeTypesPageTitles.AffordableRent,
            BuildHomeTypePage(HomeTypePagesUrl.ExemptFromTheRightToSharedOwnership, GeneralHomeType),
            ("MarketValue", homeType.MarketValue.ToString(CultureInfo.InvariantCulture)),
            ("MarketRent", homeType.MarketRent.ToString(CultureInfo.InvariantCulture)),
            ("ProspectiveRent", homeType.ProspectiveRent.ToString(CultureInfo.InvariantCulture)),
            ("TargetRentExceedMarketRent", homeType.Exceeds80PercentOfMarketRent.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ProvideExemptFromTheRightToSharedOwnership()
    {
        // given
        var homeType = GeneralHomeType.GenerateExemptFromTheRightToSharedOwnership();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.ExemptFromTheRightToSharedOwnership, GeneralHomeType),
            HomeTypesPageTitles.ExemptFromTheRightToSharedOwnership,
            BuildHomeTypePage(HomeTypePagesUrl.ExemptionJustification, GeneralHomeType),
            ("ExemptFromTheRightToSharedOwnership", homeType.ExemptFromTheRightToSharedOwnership.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ProvideExemptionJustification()
    {
        // given
        var homeType = GeneralHomeType.GenerateExemptionJustification();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.ExemptionJustification, GeneralHomeType),
            HomeTypesPageTitles.ExemptionJustification,
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction, GeneralHomeType),
            ("MoreInformation", homeType.ExemptionJustification));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ProvideModernMethodsOfConstruction()
    {
        // given
        var homeType = GeneralHomeType.GenerateModernMethodsOfConstruction();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstruction,
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstructionCategories, GeneralHomeType),
            ("ModernMethodsConstructionApplied", homeType.ModernMethodsOfConstruction.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ProvideModernMethodsConstructionCategory()
    {
        // given
        var homeType = GeneralHomeType.GenerateModernMethodsConstructionCategory();

        // when & then
        await TestPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstructionCategories, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstructionCategories,
            BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType),
            ("ModernMethodsConstructionCategories", homeType.ModernMethodsConstructionCategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_CheckAnswersHasValidSummary()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType));
        checkAnswersPage
            .UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasGdsSubmitButton("continue-button", out _);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Home type name").WhoseValue.Should().Be(GeneralHomeType.Name);
        summary.Should().ContainKey("Type of home").WhoseValue.Should().Be(GeneralHomeType.HousingType.GetDescription());
        summary.Should().ContainKey("Number of homes").WhoseValue.Should().Be(GeneralHomeType.NumberOfHomes.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of bedrooms").WhoseValue.Should().Be(GeneralHomeType.NumberOfBedrooms.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Maximum occupancy").WhoseValue.Should().Be(GeneralHomeType.MaximumOccupancy.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of storeys").WhoseValue.Should().Be(GeneralHomeType.NumberOfStoreys.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Move on accommodation").WhoseValue.Should().Be(GeneralHomeType.MoveOnAccommodation.GetDescription());
        summary.Should().ContainKey("Building type").WhoseValue.Should().Be(GeneralHomeType.BuildingType.GetDescription());
        summary.Should().ContainKey("Custom built").WhoseValue.Should().Be(GeneralHomeType.CustomBuild.GetDescription());
        summary.Should().ContainKey("Type of facilities").WhoseValue.Should().Be(GeneralHomeType.FacilityType.GetDescription());
        summary.Should().ContainKey("Accessibility categories met").WhoseValue.Should().Be(GeneralHomeType.AccessibilityStandards.GetDescription());
        summary.Should().ContainKey("Accessibility categories").WhoseValue.Should().Be(GeneralHomeType.AccessibilityCategory.GetDescription());
        summary.Should().ContainKey("Square metres of internal floor area").WhoseValue.Should().Be($"{GeneralHomeType.FloorArea.ToString("0.##", CultureInfo.InvariantCulture)}m\u00b2");
        summary.Should().ContainKey("Nationally Described Space Standards met").WhoseValue.Should().Be(GeneralHomeType.MeetSpaceStandards.GetDescription());
        summary.Should().ContainKey("Nationally Described Space Standards").WhoseValue.Should().Be(GeneralHomeType.SpaceStandards.GetDescription());
        summary.Should().ContainKey("Market value of each home").WhoseValue.Should().Be($"\u00a3{GeneralHomeType.MarketValue}");
        summary.Should().ContainKey("Market rent per week").WhoseValue.Should().Be($"\u00a3{GeneralHomeType.MarketRent.ToString("0.##", CultureInfo.InvariantCulture)}");
        summary.Should().ContainKey("Affordable rent per week").WhoseValue.Should().Be($"\u00a3{GeneralHomeType.ProspectiveRent.ToString("0.##", CultureInfo.InvariantCulture)}");
        summary.Should().ContainKey("Affordable rent as percentage of market rent").WhoseValue.Should().Be(GeneralHomeType.ProspectiveRentPercentage);
        summary.Should().ContainKey("Target rent exceeded 80% of market rent").WhoseValue.Should().Be(GeneralHomeType.Exceeds80PercentOfMarketRent.GetDescription());
        summary.Should().ContainKey("Exempt from Right to Shared ownership").WhoseValue.Should().Be(GeneralHomeType.ExemptFromTheRightToSharedOwnership.GetDescription());
        summary.Should().ContainKey("Right to Shared Ownership criteria").WhoseValue.Should().Be(GeneralHomeType.ExemptionJustification);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(18)]
    public async Task Order18_CheckAnswersCompleteHomeType()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType));
        checkAnswersPage
            .UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var homeTypeListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", YesNoType.Yes.ToString()));

        // then
        homeTypeListPage.UrlEndWith(HomeTypesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasLinkWithId("add-home-type", out _)
            .HasElementWithText($"HomeType-{GeneralHomeType.Id}", GeneralHomeType.Name);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(19)]
    public async Task Order19_CompleteHomeTypesSection()
    {
        // given
        var homeTypeListPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.List));
        homeTypeListPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var finishHomeTypesPage = await TestClient.SubmitButton(continueButton);

        // then
        finishHomeTypesPage.UrlEndWith(HomeTypesPagesUrl.FinishHomeTypes(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.FinishHomeTypes);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(20)]
    public async Task Order20_ConfirmCompleteHomeTypesSection()
    {
        // given
        var finishHomeTypesPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.FinishHomeTypes));
        finishHomeTypesPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.FinishHomeTypes))
            .HasTitle(HomeTypesPageTitles.FinishHomeTypes)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(continueButton, ("FinishAnswer", "Yes"));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId))
            .HasElementWithText("add-home-type-status", "Completed");
        SaveCurrentPage();
    }

    private string BuildHomeTypesPage(Func<string, string> homeTypesPageUrlFactory)
    {
        return homeTypesPageUrlFactory(ApplicationData.ApplicationId);
    }

    private string BuildHomeTypePage(Func<string, string, string> homeTypePageUrlFactory, INestedItemData nestedItemData)
    {
        return homeTypePageUrlFactory(ApplicationData.ApplicationId, nestedItemData.Id);
    }
}
