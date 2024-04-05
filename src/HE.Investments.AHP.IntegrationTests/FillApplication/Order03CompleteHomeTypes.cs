using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.HomeTypes.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data.HomeTypes;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03CompleteHomeTypes : AhpIntegrationTest
{
    private readonly HomeTypesData _homeTypesData;

    private readonly int _numberOfHomesPerHomeType;

    public Order03CompleteHomeTypes(IntegrationTestFixture<Program> fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        var homeTypesData = GetSharedDataOrNull<HomeTypesData>(nameof(_homeTypesData));
        if (homeTypesData is null)
        {
            homeTypesData = new HomeTypesData();
            SetSharedData(nameof(_homeTypesData), homeTypesData);
        }

        _homeTypesData = homeTypesData;

        var schemaInformationData = GetSharedDataOrNull<SchemeInformationData>(nameof(SchemeInformationData));
        _numberOfHomesPerHomeType = (schemaInformationData?.HousesToDeliver ?? 10) / 2;
    }

    private GeneralHomeTypeData GeneralHomeType => _homeTypesData.General;

    private HomesForDisabledPeopleData DisabledHomeType => _homeTypesData.Disabled;

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_HomeTypesLandingPage()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithTestId("add-home-type", out var enterHomeTypesSection);

        // when
        var landingPage = await TestClient.NavigateTo(enterHomeTypesSection);

        // then
        var continueButton = landingPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.LandingPage))
            .HasTitle(HomeTypesPageTitles.LandingPage)
            .GetLinkButton();

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
            .HasLinkButtonForTestId("add-home-type", out var enterNewHomeTypePage);

        // when
        var newHomeTypePage = await TestClient.NavigateTo(enterNewHomeTypePage);

        // then
        newHomeTypePage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.NewHomeType))
            .HasTitle(HomeTypesPageTitles.HomeTypeDetails)
            .HasSaveAndContinueButton(out var continueButton);

        var homeType = GeneralHomeType.GenerateHomeTypeDetails();
        var homeInformationPage = await TestClient.SubmitButton(
            continueButton,
            ("HomeTypeName", homeType.Name),
            ("HousingType", homeType.HousingType.ToString()));

        GeneralHomeType.SetHomeTypeId(homeInformationPage.Url.GetNestedGuidFromUrl());
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
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.HomeInformation, GeneralHomeType),
            HomeTypesPageTitles.HomeInformation,
            BuildHomeTypePage(HomeTypePagesUrl.MoveOnAccommodation, GeneralHomeType),
            ("NumberOfHomes", _numberOfHomesPerHomeType.ToString(CultureInfo.InvariantCulture)),
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.AffordableRent, GeneralHomeType),
            HomeTypesPageTitles.AffordableRent,
            BuildHomeTypePage(HomeTypePagesUrl.ExemptFromTheRightToSharedOwnership, GeneralHomeType),
            ("MarketValue", homeType.MarketValue.ToString(CultureInfo.InvariantCulture)),
            ("MarketRentPerWeek", homeType.MarketRentPerWeek.ToString(CultureInfo.InvariantCulture)),
            ("RentPerWeek", homeType.RentPerWeek.ToString(CultureInfo.InvariantCulture)),
            ("TargetRentExceedMarketRent", homeType.Exceeds80PercentOfMarketRent.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ProvideExemptFromTheRightToSharedOwnership()
    {
        // given
        var homeType = GeneralHomeType.GenerateExemptFromTheRightToSharedOwnership();

        // when & then
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstruction,
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstructionCategories, GeneralHomeType),
            ("ModernMethodsConstructionApplied", homeType.ModernMethodsOfConstruction.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ProvideModernMethodsConstructionCategories()
    {
        // given
        var homeType = GeneralHomeType.GenerateModernMethodsConstructionCategories();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstructionCategories, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstructionCategories,
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction3DSubcategories, GeneralHomeType),
            homeType.MmcCategories.ToFormInputs("ModernMethodsConstructionCategories"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_ProvideModernMethodsConstruction3DSubcategories()
    {
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction3DSubcategories, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstruction3DCategories,
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction2DSubcategories, GeneralHomeType),
            ("ModernMethodsConstruction3DSubcategories", GeneralHomeType.Mmc3DSubcategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(18)]
    public async Task Order18_ProvideModernMethodsConstruction2DSubcategories()
    {
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.ModernMethodsConstruction2DSubcategories, GeneralHomeType),
            HomeTypesPageTitles.ModernMethodsConstruction2DCategories,
            BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType),
            ("ModernMethodsConstruction2DSubcategories", GeneralHomeType.Mmc2DSubcategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(19)]
    public async Task Order19_GeneralCheckAnswersHasValidSummary()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType));
        checkAnswersPage
            .UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasSaveAndContinueButton();

        // when
        var summary = checkAnswersPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Home type name").WithValue(GeneralHomeType.Name);
        summary.Should().ContainKey("Type of home").WithValue(GeneralHomeType.HousingType);
        summary.Should().ContainKey("Number of homes").WithValue(_numberOfHomesPerHomeType.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of bedrooms").WithValue(GeneralHomeType.NumberOfBedrooms.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Maximum occupancy").WithValue(GeneralHomeType.MaximumOccupancy.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of storeys").WithValue(GeneralHomeType.NumberOfStoreys.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Move on accommodation").WithValue(GeneralHomeType.MoveOnAccommodation);
        summary.Should().ContainKey("Building type").WithValue(GeneralHomeType.BuildingType);
        summary.Should().ContainKey("Custom built").WithValue(GeneralHomeType.CustomBuild);
        summary.Should().ContainKey("Type of facilities").WithValue(GeneralHomeType.FacilityType);
        summary.Should().ContainKey("Accessibility categories met").WithValue(GeneralHomeType.AccessibilityStandards);
        summary.Should().ContainKey("Accessibility categories").WithValue(GeneralHomeType.AccessibilityCategory);
        summary.Should()
            .ContainKey("Square metres of internal floor area")
            .WithValue($"{GeneralHomeType.FloorArea.ToString("0.##", CultureInfo.InvariantCulture)}m\u00b2");
        summary.Should().ContainKey("Nationally Described Space Standards met").WithValue(GeneralHomeType.MeetSpaceStandards);
        summary.Should().ContainKey("Nationally Described Space Standards").WithValue(GeneralHomeType.SpaceStandards);
        summary.Should().ContainKey("Market value of each home").WhoseValue.Value.Should().BePoundsOnly(GeneralHomeType.MarketValue);
        summary.Should().ContainKey("Market rent per week").WhoseValue.Value.Should().BePoundsPences(GeneralHomeType.MarketRentPerWeek);
        summary.Should().ContainKey("Affordable rent per week").WhoseValue.Value.Should().BePoundsPences(GeneralHomeType.RentPerWeek);
        summary.Should().ContainKey("Affordable rent as percentage of market rent").WithValue(GeneralHomeType.ProspectiveRentPercentage);
        summary.Should().ContainKey("Target rent exceeded 80% of market rent").WithValue(GeneralHomeType.Exceeds80PercentOfMarketRent);
        summary.Should().ContainKey("Exempt from Right to Shared ownership").WithValue(GeneralHomeType.ExemptFromTheRightToSharedOwnership);
        summary.Should().ContainKey("Right to Shared Ownership criteria").WithValue(GeneralHomeType.ExemptionJustification);
        summary.Should().ContainKey("Using MMC").WithValue(GeneralHomeType.ModernMethodsOfConstruction);
        summary.Should().ContainKey("MMC categories used").WithOnlyValues(GeneralHomeType.MmcCategories);
        summary.Should().ContainKey("Sub-categories of 3D primary structural systems").WithValue(GeneralHomeType.Mmc3DSubcategory);
        summary.Should().ContainKey("Sub-categories of 2D primary structural systems").WithValue(GeneralHomeType.Mmc2DSubcategory);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(20)]
    public async Task Order20_GeneralCheckAnswersCompleteHomeType()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType));
        checkAnswersPage
            .UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, GeneralHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var homeTypeListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", YesNoType.Yes.ToString()));

        // then
        homeTypeListPage.UrlEndWith(HomeTypesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasLinkButtonForTestId("add-home-type", out _)
            .HasHomeTypeItem(GeneralHomeType.Id, GeneralHomeType.Name, out _);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(21)]
    public async Task Order21_DuplicateHomeType()
    {
        // given
        var homeTypeListPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.List));
        homeTypeListPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasDuplicateHomeTypeLink(GeneralHomeType.Id, out var duplicateButton);

        // when
        homeTypeListPage = await TestClient.NavigateTo(duplicateButton);

        // then
        homeTypeListPage.UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasHomeTypeItem(out var duplicatedHomeTypeId, out var duplicatedHomeTypeName)
            .HasHomeTypeItem(GeneralHomeType.Id, GeneralHomeType.Name, out _);

        duplicatedHomeTypeId.Should().NotBe(GeneralHomeType.Id);
        duplicatedHomeTypeName.Should().NotBe(GeneralHomeType.Name);

        _homeTypesData.DuplicateGeneralAsDisabled(duplicatedHomeTypeId, duplicatedHomeTypeName);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(22)]
    public async Task Order22_EditDuplicatedHomeType()
    {
        // given
        var homeTypeListPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.List));
        homeTypeListPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasHomeTypeItem(DisabledHomeType.Id, DisabledHomeType.Name, out var editHomeTypeButton);

        // when
        var checkAnswersPage = await TestClient.NavigateTo(editHomeTypeButton);

        // then
        checkAnswersPage.UrlEndWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasChangeAnswerSummaryButton("Home type name", out var changeNameButton);

        var homeTypeDetailsPage = await TestClient.NavigateTo(changeNameButton);
        homeTypeDetailsPage
            .UrlWithoutQueryEndsWith(BuildHomeTypePage(HomeTypePagesUrl.HomeTypeDetails, DisabledHomeType))
            .HasTitle(HomeTypesPageTitles.HomeTypeDetails)
            .HasSaveAndContinueButton();

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(23)]
    public async Task Order23_ProvideHomeTypeDetails()
    {
        // given
        var homeType = DisabledHomeType.GenerateHomeTypeDetails();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.HomeTypeDetails, DisabledHomeType),
            HomeTypesPageTitles.HomeTypeDetails,
            BuildHomeTypePage(HomeTypePagesUrl.DisabledPeople, DisabledHomeType),
            ("HomeTypeName", homeType.Name),
            ("HousingType", homeType.HousingType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(24)]
    public async Task Order24_ProvideDisabledPeopleHousingType()
    {
        // given
        var homeType = DisabledHomeType.GenerateDisabledPeopleHousingType();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.DisabledPeople, DisabledHomeType),
            HomeTypesPageTitles.DisabledPeople,
            BuildHomeTypePage(HomeTypePagesUrl.DisabledPeopleClientGroup, DisabledHomeType),
            ("HousingType", homeType.DisabledPeopleHousingType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(25)]
    public async Task Order25_ProvideDisabledPeopleClientGroup()
    {
        // given
        var homeType = DisabledHomeType.GenerateClientGroup();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.DisabledPeopleClientGroup, DisabledHomeType),
            HomeTypesPageTitles.DisabledPeopleClientGroup,
            BuildHomeTypePage(HomeTypePagesUrl.HappiDesignPrinciples, DisabledHomeType),
            ("DisabledPeopleClientGroup", homeType.ClientGroup.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(26)]
    public async Task Order26_ProvideHappiDesignPrinciples()
    {
        // given
        var homeType = DisabledHomeType.GenerateHappiDesignPrinciple();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.HappiDesignPrinciples, DisabledHomeType),
            HomeTypesPageTitles.HappiDesignPrinciples,
            BuildHomeTypePage(HomeTypePagesUrl.DesignPlans, DisabledHomeType),
            ("OtherPrinciples", homeType.HappiDesignPrinciple.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(27)]
    public async Task Order27_ProvideDesignPlans()
    {
        // given
        var homeType = DisabledHomeType.GenerateDesignPlans();
        var continueButton = await GivenTestQuestionPage(BuildHomeTypePage(HomeTypePagesUrl.DesignPlans, DisabledHomeType), HomeTypesPageTitles.DesignPlans);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            new[] { new KeyValuePair<string, string>("MoreInformation", homeType.DesignPlanInformation) },
            new[] { ("File", homeType.DesignFile) });

        // then
        ThenTestQuestionPage(nextPage, BuildHomeTypePage(HomeTypePagesUrl.SupportedHousingInformation, DisabledHomeType));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(28)]
    public async Task Order28_ProvideSupportedHousingInformation()
    {
        // given
        var homeType = DisabledHomeType.GenerateSupportedHousingInformation();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.SupportedHousingInformation, DisabledHomeType),
            HomeTypesPageTitles.SupportedHousingInformation,
            BuildHomeTypePage(HomeTypePagesUrl.RevenueFunding, DisabledHomeType),
            ("LocalCommissioningBodiesConsulted", homeType.LocalCommissioningBodiesConsulted.ToString()),
            ("ShortStayAccommodation", homeType.ShortStayAccommodation.ToString()),
            ("RevenueFundingType", homeType.RevenueFundingType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(29)]
    public async Task Order29_ProvideRevenueFundingSource()
    {
        // given
        var homeType = DisabledHomeType.GenerateRevenueFundingSource();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.RevenueFunding, DisabledHomeType),
            HomeTypesPageTitles.RevenueFunding,
            BuildHomeTypePage(HomeTypePagesUrl.MoveOnArrangements, DisabledHomeType),
            ("Sources", homeType.RevenueFundingSource.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(30)]
    public async Task Order30_ProvideMoveOnArrangements()
    {
        // given
        var homeType = DisabledHomeType.GenerateMoveOnArrangements();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.MoveOnArrangements, DisabledHomeType),
            HomeTypesPageTitles.MoveOnArrangements,
            BuildHomeTypePage(HomeTypePagesUrl.ExitPlan, DisabledHomeType),
            ("MoreInformation", homeType.MoveOnArrangements));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(31)]
    public async Task Order31_ProvideExitPlan()
    {
        // given
        var homeType = DisabledHomeType.GenerateExitPlan();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.ExitPlan, DisabledHomeType),
            HomeTypesPageTitles.ExitPlan,
            BuildHomeTypePage(HomeTypePagesUrl.TypologyLocationAndDesign, DisabledHomeType),
            ("MoreInformation", homeType.ExitPlan));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(32)]
    public async Task Order32_ProvideTypologyLocationAndDesign()
    {
        // given
        var homeType = DisabledHomeType.GenerateTypologyLocationAndDesign();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.TypologyLocationAndDesign, DisabledHomeType),
            HomeTypesPageTitles.TypologyLocationAndDesign,
            BuildHomeTypePage(HomeTypePagesUrl.PeopleGroupForSpecificDesignFeatures, DisabledHomeType),
            ("MoreInformation", homeType.TypologyLocationAndDesign));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(33)]
    public async Task Order33_ProvidePeopleGroupForSpecificDesignFeatures()
    {
        // given
        var homeType = DisabledHomeType.GeneratePeopleGroupForSpecificDesignFeatures();

        // when & then
        await TestQuestionPage(
            BuildHomeTypePage(HomeTypePagesUrl.PeopleGroupForSpecificDesignFeatures, DisabledHomeType),
            HomeTypesPageTitles.PeopleGroupForSpecificDesignFeatures,
            BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType),
            ("PeopleGroupForSpecificDesignFeatures", homeType.PeopleGroupForSpecificDesignFeatures.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(34)]
    public async Task Order34_DisabledPeopleCheckAnswersHasValidSummary()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasSaveAndContinueButton();

        // when
        var summary = checkAnswersPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Home type name").WithValue(DisabledHomeType.Name);
        summary.Should().ContainKey("Type of home").WithValue(DisabledHomeType.HousingType);
        summary.Should().ContainKey("Disabled and vulnerable people - Type of home").WithValue(DisabledHomeType.DisabledPeopleHousingType);
        summary.Should().ContainKey("Client group").WithValue(DisabledHomeType.ClientGroup);
        summary.Should().ContainKey("HAPPI principles").WithValue(DisabledHomeType.HappiDesignPrinciple);
        summary.Should()
            .ContainKey("Design plans")
            .WhoseValue.Value.Should()
            .ContainAll(DisabledHomeType.DesignPlanInformation, DisabledHomeType.DesignFile.Name);
        summary.Should().ContainKey("Local commissioning bodies consultation").WithValue(DisabledHomeType.LocalCommissioningBodiesConsulted);
        summary.Should().ContainKey("Short stay").WithValue(DisabledHomeType.ShortStayAccommodation);
        summary.Should().ContainKey("Revenue funding").WithValue(DisabledHomeType.RevenueFundingType);
        summary.Should().ContainKey("Sources of revenue funding").WithValue(DisabledHomeType.RevenueFundingSource);
        summary.Should().ContainKey("Move on arrangements in place").WithValue(DisabledHomeType.MoveOnArrangements);
        summary.Should().ContainKey("Exit plan or alternative use").WithValue(DisabledHomeType.ExitPlan);
        summary.Should().ContainKey("Typology, location and design").WithValue(DisabledHomeType.TypologyLocationAndDesign);
        summary.Should().ContainKey("Number of homes").WithValue(_numberOfHomesPerHomeType.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of bedrooms").WithValue(DisabledHomeType.NumberOfBedrooms.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Maximum occupancy").WithValue(DisabledHomeType.MaximumOccupancy.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Number of storeys").WithValue(DisabledHomeType.NumberOfStoreys.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Particular group").WithValue(DisabledHomeType.PeopleGroupForSpecificDesignFeatures);
        summary.Should().ContainKey("Building type").WithValue(DisabledHomeType.BuildingType);
        summary.Should().ContainKey("Custom built").WithValue(DisabledHomeType.CustomBuild);
        summary.Should().ContainKey("Type of facilities").WithValue(DisabledHomeType.FacilityType);
        summary.Should().ContainKey("Accessibility categories met").WithValue(DisabledHomeType.AccessibilityStandards);
        summary.Should().ContainKey("Accessibility categories").WithValue(DisabledHomeType.AccessibilityCategory);
        summary.Should()
            .ContainKey("Square metres of internal floor area")
            .WithValue($"{DisabledHomeType.FloorArea.ToString("0.##", CultureInfo.InvariantCulture)}m\u00b2");
        summary.Should().ContainKey("Nationally Described Space Standards met").WithValue(DisabledHomeType.MeetSpaceStandards);
        summary.Should().ContainKey("Nationally Described Space Standards").WithValue(DisabledHomeType.SpaceStandards);
        summary.Should().ContainKey("Market value of each home").WhoseValue.Value.Should().BePoundsOnly(DisabledHomeType.MarketValue);
        summary.Should().ContainKey("Market rent per week").WhoseValue.Value.Should().BePoundsPences(DisabledHomeType.MarketRentPerWeek);
        summary.Should().ContainKey("Affordable rent per week").WhoseValue.Value.Should().BePoundsPences(DisabledHomeType.RentPerWeek);
        summary.Should().ContainKey("Affordable rent as percentage of market rent").WithValue(DisabledHomeType.ProspectiveRentPercentage);
        summary.Should().ContainKey("Target rent exceeded 80% of market rent").WithValue(DisabledHomeType.Exceeds80PercentOfMarketRent);
        summary.Should().ContainKey("Exempt from Right to Shared ownership").WithValue(DisabledHomeType.ExemptFromTheRightToSharedOwnership);
        summary.Should().ContainKey("Right to Shared Ownership criteria").WithValue(DisabledHomeType.ExemptionJustification);
        summary.Should().ContainKey("Using MMC").WithValue(GeneralHomeType.ModernMethodsOfConstruction);
        summary.Should().ContainKey("MMC categories used").WithOnlyValues(GeneralHomeType.MmcCategories);
        summary.Should().ContainKey("Sub-categories of 3D primary structural systems").WithValue(GeneralHomeType.Mmc3DSubcategory);
        summary.Should().ContainKey("Sub-categories of 2D primary structural systems").WithValue(GeneralHomeType.Mmc2DSubcategory);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(35)]
    public async Task Order35_DisabledPeopleCheckAnswersCompleteHomeType()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(BuildHomeTypePage(HomeTypePagesUrl.CheckAnswers, DisabledHomeType))
            .HasTitle(HomeTypesPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var homeTypeListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", YesNoType.Yes.ToString()));

        // then
        homeTypeListPage.UrlEndWith(HomeTypesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasLinkButtonForTestId("add-home-type", out _)
            .HasHomeTypeItem(DisabledHomeType.Id, DisabledHomeType.Name, out _)
            .HasHomeTypeItem(GeneralHomeType.Id, GeneralHomeType.Name, out _);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(36)]
    public async Task Order36_CompleteHomeTypesSection()
    {
        // given
        var homeTypeListPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.List));
        homeTypeListPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.List))
            .HasTitle(HomeTypesPageTitles.HomeTypes)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var finishHomeTypesPage = await TestClient.SubmitButton(continueButton);

        // then
        finishHomeTypesPage.UrlEndWith(HomeTypesPagesUrl.FinishHomeTypes(ApplicationData.ApplicationId))
            .HasTitle(HomeTypesPageTitles.FinishHomeTypes);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(37)]
    public async Task Order37_ConfirmCompleteHomeTypesSection()
    {
        // given
        var finishHomeTypesPage = await GetCurrentPage(BuildHomeTypesPage(HomeTypesPagesUrl.FinishHomeTypes));
        finishHomeTypesPage
            .UrlEndWith(BuildHomeTypesPage(HomeTypesPagesUrl.FinishHomeTypes))
            .HasTitle(HomeTypesPageTitles.FinishHomeTypes)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(continueButton, ("FinishAnswer", "Yes"));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId))
            .HasSectionWithStatus("add-home-type-status", "Completed");
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
