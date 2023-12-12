using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc;
using Controller = HE.Investment.AHP.WWW.Controllers.HomeTypesController;
using Workflow = HE.Investment.AHP.Domain.HomeTypes.HomeTypesWorkflowState;

namespace HE.Investment.AHP.WWW.Models.HomeTypes.Factories;

public class HomeTypeSummaryViewModelFactory : IHomeTypeSummaryViewModelFactory
{
    public IEnumerable<SectionSummaryViewModel> CreateSummaryModel(FullHomeType homeType, IUrlHelper urlHelper, bool useWorkflowRedirection = false)
    {
        var factory = new HomeTypeQuestionFactory(homeType, urlHelper, useWorkflowRedirection);

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
            x => DownloadDesignFileUrl(urlHelper, homeType.ApplicationId, homeType.Id, x.FileName));
        return SectionSummaryViewModel.New(
            "Design Plans",
            factory.FileQuestion("Design Plans", nameof(Controller.DesignPlans), designPlans.MoreInformation, files));
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
            factory.Question("Market value of each home", nameof(Controller.AffordableRent), ToPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.AffordableRent), ToPoundsPences(tenure.MarketRent)),
            factory.Question("Affordable rent per week", nameof(Controller.AffordableRent), ToPoundsPences(tenure.ProspectiveRent)),
            factory.Question("Affordable rent as percentage of market rent", nameof(Controller.AffordableRent), ToPercentage(tenure.CalculatedProspectivePercentage)),
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
            factory.Question("Market value of each home", nameof(Controller.SocialRent), ToPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.SocialRent), ToPoundsPences(tenure.MarketRent)),
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
            factory.Question("Market value of each home", nameof(Controller.SharedOwnership), ToPounds(tenure.MarketValue)),
            factory.Question("Average first tranche sale percentage", nameof(Controller.SharedOwnership), ToPercentage(tenure.InitialSalePercentage)),
            factory.Question("First tranche sales receipt", nameof(Controller.SharedOwnership), ToPoundsPences(tenure.ExpectedFirstTranche)),
            factory.Question("Shared Ownership rent per week", nameof(Controller.SharedOwnership), ToPoundsPences(tenure.ProspectiveRent)),
            factory.Question(
                "Shared Ownership rent as percentage of the unsold share",
                nameof(Controller.SharedOwnership),
                ToPercentage(tenure.SharedOwnershipRentAsPercentageOfTheUnsoldShare)),
            factory.DeadEnd(nameof(Controller.SharedOwnershipIneligible)));
    }

    private static SectionSummaryViewModel CreateRentToBuySection(TenureDetails tenure, HomeTypeQuestionFactory factory)
    {
        return SectionSummaryViewModel.New(
            "Rent to Buy details",
            factory.Question("Market value of each home", nameof(Controller.RentToBuy), ToPounds(tenure.MarketValue)),
            factory.Question("Market rent per week", nameof(Controller.RentToBuy), ToPoundsPences(tenure.MarketRent)),
            factory.Question("Rent per week", nameof(Controller.RentToBuy), ToPoundsPences(tenure.ProspectiveRent)),
            factory.Question("Rent as percentage of market rent", nameof(Controller.RentToBuy), ToPercentage(tenure.CalculatedProspectivePercentage)),
            factory.Question("Target rent exceed 80% of market rent", nameof(Controller.RentToBuy), tenure.TargetRentExceedMarketRent),
            factory.DeadEnd(nameof(Controller.RentToBuyIneligible)));
    }

    private static string DownloadDesignFileUrl(IUrlHelper urlHelper, string applicationId, string homeTypeId, string fileId)
    {
        return urlHelper.RouteUrl(
            "subSection",
            new
            {
                controller = "HomeTypes",
                action = "DownloadDesignPlansFile",
                applicationId,
                id = homeTypeId,
                fileId,
            }) ?? string.Empty;
    }

    private static string? ToPounds(int? value) => value?.ToString("\u00a30", CultureInfo.InvariantCulture);

    private static string? ToPoundsPences(decimal? value) => value?.ToString("\u00a300.##", CultureInfo.InvariantCulture);

    private static string? ToPercentage(decimal? value) => value?.ToString("00.00\\%", CultureInfo.InvariantCulture);

    private static string? ToSquareMeters(decimal? value) => value?.ToString("0.##m\u00B2", CultureInfo.InvariantCulture);
}
