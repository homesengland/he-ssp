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
            factory.Question("Custom built", nameof(Controller.CustomBuildProperty), homeInformation.CustomBuild),
            factory.Question("Type of facilities", nameof(Controller.TypeOfFacilities), homeInformation.FacilityType),
            factory.Question("Accessibility categories met", nameof(Controller.AccessibilityStandards), homeInformation.AccessibilityStandards),
            factory.Question("Accessibility categories", nameof(Controller.AccessibilityCategory), homeInformation.AccessibilityCategory));
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
}
