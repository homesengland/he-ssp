using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc;
using Controller = HE.Investment.AHP.WWW.Controllers.HomeTypesController;
using Workflow = HE.Investment.AHP.Contract.HomeTypes.HomeTypesWorkflowState;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

public class HomeTypeSummaryViewModelFactory : IHomeTypeSummaryViewModelFactory
{
    public IEnumerable<SectionSummaryViewModel> CreateSummaryModel(
        FullHomeType homeType,
        IUrlHelper urlHelper,
        bool isReadOnly,
        bool useWorkflowRedirection = false)
    {
        return CreateSummarySectionsModel(homeType, urlHelper, isReadOnly, useWorkflowRedirection)
            .Where(x => x.Items != null && x.Items.Any());
    }

    private static IEnumerable<SectionSummaryViewModel> CreateSummarySectionsModel(
        FullHomeType homeType,
        IUrlHelper urlHelper,
        bool isReadOnly,
        bool useWorkflowRedirection)
    {
        var factory = new HomeTypeQuestionFactory(homeType, urlHelper, isReadOnly, useWorkflowRedirection);

        yield return CreateHomeTypeDetailsSection(homeType, factory);

        if (homeType.DisabledPeople != null)
        {
            yield return CreateDisabledPeopleSection(homeType.DisabledPeople, homeType.DesignPlans, factory);
        }

        if (homeType.OlderPeople != null)
        {
            yield return CreateOlderPeopleSection(homeType.OlderPeople, homeType.DesignPlans, factory);
        }

        if (homeType.DesignPlans != null)
        {
            yield return CreateDesignPlansSection(homeType, homeType.DesignPlans, factory, urlHelper);
        }

        if (homeType.SupportedHousing != null)
        {
            yield return CreateSupportedHousingSection(homeType.SupportedHousing, factory);
        }

        yield return CreateHomeInformationSection(homeType.HomeInformation, factory);
        yield return CreateBuildingInformationSection(homeType.HomeInformation, factory);
        yield return CreateFloorAreaSection(homeType.HomeInformation, factory);

        if (homeType.Tenure is Tenure.AffordableRent)
        {
            yield return CreateAffordableRentSection(homeType.TenureDetails, factory);
        }
        else if (homeType.Tenure is Tenure.SocialRent)
        {
            yield return CreateSocialRentSection(homeType.TenureDetails, factory);
        }
        else if (homeType.Tenure is Tenure.SharedOwnership)
        {
            yield return CreateSharedOwnershipSection(homeType.TenureDetails, factory);
        }
        else if (homeType.Tenure is Tenure.RentToBuy)
        {
            yield return CreateRentToBuySection(homeType.TenureDetails, factory);
        }
        else if (homeType.Tenure is Tenure.HomeOwnershipLongTermDisabilities)
        {
            yield return CreateHomeOwnershipDisabilitiesSection(homeType.TenureDetails, factory);
        }
        else if (homeType.Tenure is Tenure.OlderPersonsSharedOwnership)
        {
            yield return CreateOlderPersonsSharedOwnershipSection(homeType.TenureDetails, factory);
        }

        yield return CreateModernMethodsConstructionSection(homeType.ModernMethodsConstruction, factory);
    }

    private static SectionSummaryViewModel CreateHomeTypeDetailsSection(FullHomeType homeType, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Home type details",
            factory.Question("Home type name", nameof(Controller.HomeTypeDetails), homeType.Name),
            factory.Question("Type of home", nameof(Controller.HomeTypeDetails), homeType.HousingType));
    }

    private static SectionSummaryViewModel CreateDisabledPeopleSection(DisabledPeopleHomeTypeDetails disabledPeople, DesignPlans? designPlans, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Disabled and vulnerable people",
            factory.Question("Type of home", nameof(Controller.HomesForDisabledPeople), disabledPeople.HousingType),
            factory.Question("Client group", nameof(Controller.DisabledPeopleClientGroup), disabledPeople.ClientGroupType),
            factory.Question("HAPPI principles", nameof(Controller.HappiDesignPrinciples), designPlans?.DesignPrinciples.ToArray()));
    }

    private static SectionSummaryViewModel CreateOlderPeopleSection(OlderPeopleHomeTypeDetails olderPeople, DesignPlans? designPlans, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Older people",
            factory.Question("Type of home", nameof(Controller.HomesForOlderPeople), olderPeople.HousingType),
            factory.Question("HAPPI principles", nameof(Controller.HappiDesignPrinciples), designPlans?.DesignPrinciples.ToArray()));
    }

    private static SectionSummaryViewModel CreateDesignPlansSection(FullHomeType homeType, DesignPlans designPlans, HomeTypeQuestionFactory factory, IUrlHelper urlHelper)
    {
        var files = designPlans.UploadedFiles.ToDictionary(
            x => x.FileName,
            x => DownloadDesignFileUrl(urlHelper, homeType.ApplicationId, homeType.Id, x.FileId));
        return SectionSummaryViewModel.New(
            "Design plans",
            factory.FileQuestion("Design plans", nameof(Controller.DesignPlans), designPlans.MoreInformation, files));
    }

    private static SectionSummaryViewModel CreateSupportedHousingSection(SupportedHousingInformation supportedHousing, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Supported housing information",
            factory.Question(
                "Local commissioning bodies consultation",
                nameof(Controller.SupportedHousingInformation),
                supportedHousing.LocalCommissioningBodiesConsulted),
            factory.Question("Short stay", nameof(Controller.SupportedHousingInformation), supportedHousing.ShortStayAccommodation),
            factory.Question("Revenue funding", nameof(Controller.SupportedHousingInformation), supportedHousing.RevenueFundingType),
            factory.Question("Sources of revenue funding", nameof(HomeTypesController.RevenueFunding), supportedHousing.RevenueFundingSources.ToArray()),
            factory.Question("Move on arrangements in place", nameof(Controller.MoveOnArrangements), supportedHousing.MoveOnArrangements),
            factory.Question("Exit plan or alternative use", nameof(Controller.ExitPlan), supportedHousing.ExitPlan),
            factory.Question("Typology, location and design", nameof(Controller.TypologyLocationAndDesign), supportedHousing.TypologyLocationAndDesign));
    }

    private static SectionSummaryViewModel CreateHomeInformationSection(HomeInformation homeInformation, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Home information",
            factory.Question("Number of homes", nameof(Controller.HomeInformation), homeInformation.NumberOfHomes),
            factory.Question("Number of bedrooms", nameof(Controller.HomeInformation), homeInformation.NumberOfBedrooms),
            factory.Question("Maximum occupancy", nameof(Controller.HomeInformation), homeInformation.MaximumOccupancy),
            factory.Question("Number of storeys", nameof(Controller.HomeInformation), homeInformation.NumberOfStoreys),
            factory.Question("Move on accommodation", nameof(Controller.MoveOnAccommodation), homeInformation.IntendedAsMoveOnAccommodation),
            factory.Question(
                "Particular group",
                nameof(Controller.PeopleGroupForSpecificDesignFeatures),
                homeInformation.PeopleGroupForSpecificDesignFeatures));
    }

    private static SectionSummaryViewModel CreateBuildingInformationSection(HomeInformation homeInformation, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Building information",
            factory.Question("Building type", nameof(Controller.BuildingInformation), homeInformation.BuildingType),
            factory.DeadEnd(nameof(Controller.BuildingInformationIneligible)),
            factory.Question("Custom built", nameof(Controller.CustomBuildProperty), homeInformation.CustomBuild),
            factory.Question("Type of facilities", nameof(Controller.TypeOfFacilities), homeInformation.FacilityType),
            factory.Question("Accessibility categories met", nameof(Controller.AccessibilityStandards), homeInformation.AccessibilityStandards),
            factory.Question("Accessibility categories", nameof(Controller.AccessibilityCategory), homeInformation.AccessibilityCategory));
    }

    private static SectionSummaryViewModel CreateFloorAreaSection(HomeInformation homeInformation, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Floor area",
            factory.Question("Square metres of internal floor area", nameof(Controller.FloorArea), ToSquareMeters(homeInformation.InternalFloorArea)),
            factory.Question("Nationally Described Space Standards met", nameof(Controller.FloorArea), homeInformation.MeetNationallyDescribedSpaceStandards),
            factory.Question(
                "Nationally Described Space Standards",
                nameof(Controller.FloorAreaStandards),
                homeInformation.NationallyDescribedSpaceStandards.ToArray()));
    }

    private static SectionSummaryViewModel CreateAffordableRentSection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Affordable Rent details",
            factory.Question("Market value of each home", nameof(Controller.AffordableRent), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.AffordableRent), CurrencyHelper.DisplayPoundsPences(tenure.MarketRent)),
            factory.Question("Affordable rent per week", nameof(Controller.AffordableRent), CurrencyHelper.DisplayPoundsPences(tenure.ProspectiveRent)),
            factory.Question("Affordable rent as percentage of market rent", nameof(Controller.AffordableRent), ToPercentage(tenure.ProspectiveRentAsPercentageOfMarketRent)),
            factory.Question("Target rent exceeded 80% of market rent", nameof(Controller.AffordableRent), tenure.TargetRentExceedMarketRent),
            factory.DeadEnd(nameof(Controller.AffordableRentIneligible)),
            factory.Question(
                "Exempt from Right to Shared ownership",
                nameof(Controller.ExemptFromTheRightToSharedOwnership),
                tenure.ExemptFromTheRightToSharedOwnership),
            factory.Question("Right to Shared Ownership criteria", nameof(Controller.ExemptionJustification), tenure.ExemptionJustification));
    }

    private static SectionSummaryViewModel CreateSocialRentSection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Social Rent details",
            factory.Question("Market value of each home", nameof(Controller.SocialRent), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.SocialRent), CurrencyHelper.DisplayPoundsPences(tenure.MarketRent)),
            factory.Question(
                "Exempt from Right to Shared ownership",
                nameof(Controller.ExemptFromTheRightToSharedOwnership),
                tenure.ExemptFromTheRightToSharedOwnership),
            factory.Question("Right to Shared Ownership criteria", nameof(Controller.ExemptionJustification), tenure.ExemptionJustification));
    }

    private static SectionSummaryViewModel CreateSharedOwnershipSection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Shared Ownership details",
            factory.Question("Market value of each home", nameof(Controller.SharedOwnership), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Average first tranche sale percentage", nameof(Controller.SharedOwnership), ToPercentage(tenure.InitialSale)),
            factory.Question("First tranche sales receipt", nameof(Controller.SharedOwnership), CurrencyHelper.DisplayPoundsPences(tenure.ExpectedFirstTranche)),
            factory.Question("Shared Ownership rent per week", nameof(Controller.SharedOwnership), CurrencyHelper.DisplayPoundsPences(tenure.ProspectiveRent)),
            factory.Question(
                "Shared Ownership rent as percentage of the unsold share",
                nameof(Controller.SharedOwnership),
                ToPercentage(tenure.RentAsPercentageOfTheUnsoldShare)),
            factory.DeadEnd(nameof(Controller.ProspectiveRentIneligible)));
    }

    private static SectionSummaryViewModel CreateRentToBuySection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Rent to Buy details",
            factory.Question("Market value of each home", nameof(Controller.RentToBuy), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.RentToBuy), CurrencyHelper.DisplayPoundsPences(tenure.MarketRent)),
            factory.Question("Rent per week", nameof(Controller.RentToBuy), CurrencyHelper.DisplayPoundsPences(tenure.ProspectiveRent)),
            factory.Question("Rent as percentage of market rent", nameof(Controller.RentToBuy), ToPercentage(tenure.ProspectiveRentAsPercentageOfMarketRent)),
            factory.Question("Target rent exceed 80% of market rent", nameof(Controller.RentToBuy), tenure.TargetRentExceedMarketRent),
            factory.DeadEnd(nameof(Controller.RentToBuyIneligible)));
    }

    private static SectionSummaryViewModel CreateHomeOwnershipDisabilitiesSection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "HOLD details",
            factory.Question("Market value of each home", nameof(Controller.HomeOwnershipDisabilities), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Average first tranche sale percentage", nameof(Controller.HomeOwnershipDisabilities), ToPercentage(tenure.InitialSale)),
            factory.Question("First tranche sales receipt", nameof(Controller.HomeOwnershipDisabilities), CurrencyHelper.DisplayPoundsPences(tenure.ExpectedFirstTranche)),
            factory.Question("Rent per week", nameof(Controller.HomeOwnershipDisabilities), CurrencyHelper.DisplayPoundsPences(tenure.ProspectiveRent)),
            factory.Question(
                "Rent as percentage of the unsold share",
                nameof(Controller.HomeOwnershipDisabilities),
                ToPercentage(tenure.RentAsPercentageOfTheUnsoldShare)),
            factory.DeadEnd(nameof(Controller.ProspectiveRentIneligible)));
    }

    private static SectionSummaryViewModel CreateOlderPersonsSharedOwnershipSection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "OPSO details",
            factory.Question("Market value of each home", nameof(Controller.OlderPersonsSharedOwnership), CurrencyHelper.DisplayPounds(tenure.MarketValue)),
            factory.Question("Average first tranche sale percentage", nameof(Controller.OlderPersonsSharedOwnership), ToPercentage(tenure.InitialSale)),
            factory.Question("First tranche sales receipt", nameof(Controller.OlderPersonsSharedOwnership), CurrencyHelper.DisplayPoundsPences(tenure.ExpectedFirstTranche)),
            factory.Question("Rent per week", nameof(Controller.OlderPersonsSharedOwnership), CurrencyHelper.DisplayPoundsPences(tenure.ProspectiveRent)),
            factory.Question(
                "Rent as percentage of the unsold share",
                nameof(Controller.OlderPersonsSharedOwnership),
                ToPercentage(tenure.RentAsPercentageOfTheUnsoldShare)),
            factory.DeadEnd(nameof(Controller.ProspectiveRentIneligible)));
    }

    private static SectionSummaryViewModel CreateModernMethodsConstructionSection(ModernMethodsConstruction modernMethodsConstruction, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Modern Methods of Construction (MMC)",
            factory.Question(
                "MMC categories used",
                nameof(HomeTypesController.ModernMethodsConstructionCategories),
                modernMethodsConstruction.ModernMethodsConstructionCategories.ToArray()),
            factory.Question(
                "Sub-categories of 3D primary structural systems",
                nameof(HomeTypesController.ModernMethodsConstruction3DSubcategories),
                modernMethodsConstruction.ModernMethodsConstruction3DSubcategories.ToArray()),
            factory.Question(
                "Sub-categories of 2D primary structural systems",
                nameof(HomeTypesController.ModernMethodsConstruction2DSubcategories),
                modernMethodsConstruction.ModernMethodsConstruction2DSubcategories.ToArray()));
    }

    private static string DownloadDesignFileUrl(IUrlHelper urlHelper, AhpApplicationId applicationId, HomeTypeId homeTypeId, FileId fileId)
    {
        return urlHelper.Action(
                   "DownloadDesignPlansFile",
                   "HomeTypes",
                   new { applicationId = applicationId.Value, homeTypeId = homeTypeId.Value, fileId = fileId.Value })
               ?? string.Empty;
    }

    private static string? ToPercentage(decimal? value) => value?.ToString("0.##\\%", CultureInfo.InvariantCulture);

    private static string? ToSquareMeters(decimal? value) => value?.ToString("0.##m\u00B2", CultureInfo.InvariantCulture);
}
