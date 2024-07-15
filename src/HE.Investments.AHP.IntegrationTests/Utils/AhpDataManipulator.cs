using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Crm;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.HomeTypes.Crm;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.HomeTypes;
using HE.Investments.Common.CRM.Model;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Utils;

public class AhpDataManipulator
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IHomeTypeCrmContext _homeTypeCrmContext;

    private readonly IDeliveryPhaseCrmContext _deliveryPhaseCrmContext;

    public AhpDataManipulator(
        IApplicationCrmContext applicationCrmContext,
        IHomeTypeCrmContext homeTypeCrmContext,
        IDeliveryPhaseCrmContext deliveryPhaseCrmContext)
    {
        _applicationCrmContext = applicationCrmContext;
        _homeTypeCrmContext = homeTypeCrmContext;
        _deliveryPhaseCrmContext = deliveryPhaseCrmContext;
    }

    public async Task<string> CreateAhpAllocation(
        ILoginData loginData,
        ApplicationData applicationData,
        FinancialDetailsData financialDetailsData,
        SchemeInformationData schemeInformationData,
        HomeTypesData homeTypesData,
        DeliveryPhasesData deliveryPhasesData)
    {
        var allocationId = await CreateApplication(loginData, applicationData, financialDetailsData, schemeInformationData);
        await AddHomeType(loginData, allocationId, homeTypesData);
        await AddDeliveryPhases(loginData, allocationId, deliveryPhasesData);

        return allocationId;
    }

    private async Task<string> CreateApplication(
        ILoginData loginData,
        ApplicationData applicationData,
        FinancialDetailsData financialDetailsData,
        SchemeInformationData schemeInformationData)
    {
        MethodRunner
            .New()
            .RunAllPublicMethods(applicationData.GetType())
            .RunAllPublicMethods(financialDetailsData.GetType())
            .RunAllPublicMethods(schemeInformationData.GetType());

        var applicationDto = new AhpApplicationDto
        {
            applicationPartnerConfirmation = true,
            applicationStatus = (int)invln_scheme_StatusCode.Approved,
            borrowingAgainstRentalIncomeFromThisScheme = financialDetailsData.ExpectedContributionsRentalIncomeBorrowing,
            contactExternalId = loginData.UserGlobalId,
            currentLandValue = financialDetailsData.PublicLandValue,
            dateSubmitted = DateTime.UtcNow,
            deliveryPhasesSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            developingPartnerId = schemeInformationData.DevelopingPartner.Id.ToGuidAsString(),
            developingPartnerName = schemeInformationData.DevelopingPartner.Name,
            discussionsWithLocalStakeholders = schemeInformationData.StakeholderDiscussions,
            expectedAcquisitionCost = financialDetailsData.LandStatus,
            expectedOnCosts = financialDetailsData.ExpectedOnCosts,
            expectedOnWorks = financialDetailsData.ExpectedWorksCosts,
            financialDetailsSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            fundingFromOpenMarketHomesNotOnThisScheme = financialDetailsData.ExpectedContributionsSaleOfHomesOnOtherSchemes,
            fundingFromOpenMarketHomesOnThisScheme = financialDetailsData.ExpectedContributionsSaleOfHomesOnThisScheme,
            fundingGeneratedFromOtherSources = financialDetailsData.ExpectedContributionsOtherCapitalSources,
            fundingRequested = financialDetailsData.SchemaFunding,
            homeTypesSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            howMuchReceivedFromCountyCouncil = financialDetailsData.CountyCouncilGrants,
            howMuchReceivedFromDepartmentOfHealth = financialDetailsData.HealthRelatedGrants,
            howMuchReceivedFromDhscExtraCareFunding = financialDetailsData.DhscExtraCareGrants,
            howMuchReceivedFromLocalAuthority1 = financialDetailsData.LocalAuthorityGrants,
            howMuchReceivedFromLotteryFunding = financialDetailsData.LotteryGrants,
            howMuchReceivedFromOtherPublicBodies = financialDetailsData.OtherPublicBodiesGrants,
            howMuchReceivedFromSocialServices = financialDetailsData.SocialServicesGrants,
            isPublicLand = financialDetailsData.IsPublicLand,
            meetingLocalHousingNeed = schemeInformationData.HousingNeedsMeetingLocalHousingNeed,
            meetingLocalProrities = schemeInformationData.HousingNeedsMeetingLocalPriorities,
            name = applicationData.ApplicationName,
            noOfHomes = schemeInformationData.HousesToDeliver,
            organisationId = loginData.OrganisationId,
            otherCapitalSources = financialDetailsData.ExpectedContributionsOtherCapitalSources,
            ownResources = financialDetailsData.ExpectedContributionsOwnResources,
            ownerOfTheHomesAfterCompletionId = schemeInformationData.OwnerOfTheHomes.Id.ToGuidAsString(),
            ownerOfTheHomesAfterCompletionName = schemeInformationData.OwnerOfTheHomes.Name,
            ownerOfTheLandDuringDevelopmentId = schemeInformationData.OwnerOfTheLand.Id.ToGuidAsString(),
            ownerOfTheLandDuringDevelopmentName = schemeInformationData.OwnerOfTheLand.Name,
            previousExternalStatus = (int)invln_scheme_StatusCode.ApplicationSubmitted,
            programmeId = "d5fe3baa-eeae-ee11-a569-0022480041cf",
            recycledCapitalGrantFund = financialDetailsData.ExpectedContributionsRcgfContribution,
            representationsandwarranties = true,
            schemeInformationSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            tenure = (int)invln_Tenure.Affordablerent,

            // todo set isAllocation flag
        };
        var applicationId = await _applicationCrmContext.Save(applicationDto, loginData.OrganisationId, loginData.UserGlobalId, CancellationToken.None);

        return applicationId;
    }

    private async Task AddHomeType(ILoginData loginData, string applicationId, HomeTypesData homeTypesData)
    {
        var disabledHomeTypeDto = new HomeTypeDto()
        {
            RtSOExemption = YesNoTypeMapper.Map(homeTypesData.Disabled.ExemptFromTheRightToSharedOwnership),
            accessibilityCategory = (int)invln_AccessibilitycategorySet.Category1VisitableDwelling,
            applicationId = applicationId,
            areHomesCustomBuild = YesNoTypeMapper.Map(homeTypesData.Disabled.CustomBuild),
            buildingType = (int)invln_Buildingtype.House,
            clientGroup = (int)invln_Clientgroup.APeoplewithalcoholproblems,
            designPlansMoreInformation = homeTypesData.Disabled.DesignPlanInformation,
            designPrinciples = { (int)invln_HAPPIprinciples.KNone },
            doAllHomesMeetNDSS = false,
            exemptionJustification = homeTypesData.Disabled.ExemptionJustification,
            exitPlan = homeTypesData.Disabled.ExitPlan,
            floorArea = homeTypesData.Disabled.FloorArea,
            fundingSources = { (int)invln_Revenuefundingsources.SocialServicesDepartment },
            homeTypeName = homeTypesData.Disabled.Name,
            homesDesignedForUseOfParticularGroup = (int)invln_Homesdesignedforuseofparticulargrou.Disabledpeople,
            housingType = (int)invln_Typeofhousing.Housingfordisabledandvulnerablepeople,
            housingTypeForVulnerable = (int)invln_typeofhousingfordisabledvulnerable.Designatedhousingfordisabledandvulnerablepeoplewithaccesstosupport,
            isCompleted = true,
            isWheelchairStandardMet = YesNoTypeMapper.Map(homeTypesData.Disabled.MeetSpaceStandards),
            localComissioningBodies = YesNoTypeMapper.Map(homeTypesData.Disabled.LocalCommissioningBodiesConsulted),
            marketRent = homeTypesData.Disabled.MarketRentPerWeek,
            marketValue = homeTypesData.Disabled.MarketValue,
            maxOccupancy = homeTypesData.Disabled.MaximumOccupancy,
            mmcApplied = YesNoTypeMapper.Map(homeTypesData.Disabled.ModernMethodsOfConstruction),
            mmcCategories =
            {
                (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements,
            },
            mmcCategories1Subcategories = { (int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout },
            mmcCategories2Subcategories =
            {
                (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows,
            },
            moveOnArrangements = homeTypesData.Disabled.MoveOnArrangements,
            numberOfBedrooms = homeTypesData.Disabled.NumberOfBedrooms,
            numberOfHomes = homeTypesData.Disabled.NumberOfHomes,
            numberOfStoreys = homeTypesData.Disabled.NumberOfStoreys,
            prospectiveRent = homeTypesData.Disabled.RentPerWeek,
            prospectiveRentAsPercentOfMarketRent =
                int.Parse(homeTypesData.Disabled.ProspectiveRentPercentage.Replace("%", string.Empty), CultureInfo.InvariantCulture) / 100,
            revenueFunding = (int)invln_Revenuefunding.Yesrevenuefundingisneededandhasbeenidentified,
            sharedFacilities = (int)invln_Facilities.Selfcontainedfacilities,
            shortStayAccommodation = YesNoTypeMapper.Map(homeTypesData.Disabled.ShortStayAccommodation),
            targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(homeTypesData.Disabled.Exceeds80PercentOfMarketRent),
            typologyLocationAndDesign = homeTypesData.Disabled.TypologyLocationAndDesign,
            whichNDSSStandardsHaveBeenMet = { (int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths },
        };

        var generalHomeTypeDto = new HomeTypeDto
        {
            RtSOExemption = YesNoTypeMapper.Map(homeTypesData.General.ExemptFromTheRightToSharedOwnership),
            accessibilityCategory = (int)invln_AccessibilitycategorySet.Category1VisitableDwelling,
            applicationId = applicationId,
            areHomesCustomBuild = YesNoTypeMapper.Map(homeTypesData.General.CustomBuild),
            buildingType = (int)invln_Buildingtype.House,
            doAllHomesMeetNDSS = false,
            exemptionJustification = homeTypesData.General.ExemptionJustification,
            floorArea = homeTypesData.General.FloorArea,
            homeTypeName = homeTypesData.General.Name,
            housingType = (int)invln_Typeofhousing.General,
            isCompleted = true,
            isMoveOnAccommodation = YesNoTypeMapper.Map(homeTypesData.General.MoveOnAccommodation),
            isWheelchairStandardMet = YesNoTypeMapper.Map(homeTypesData.General.MeetSpaceStandards),
            marketRent = homeTypesData.General.MarketRentPerWeek,
            marketValue = homeTypesData.General.MarketValue,
            maxOccupancy = homeTypesData.General.MaximumOccupancy,
            mmcApplied = YesNoTypeMapper.Map(homeTypesData.General.ModernMethodsOfConstruction),
            mmcCategories =
            {
                (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements,
            },
            mmcCategories1Subcategories = { (int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout },
            mmcCategories2Subcategories =
            {
                (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows,
            },
            numberOfBedrooms = homeTypesData.General.NumberOfBedrooms,
            numberOfHomes = homeTypesData.General.NumberOfHomes,
            numberOfStoreys = homeTypesData.General.NumberOfStoreys,
            prospectiveRent = homeTypesData.General.RentPerWeek,
            prospectiveRentAsPercentOfMarketRent =
                int.Parse(homeTypesData.General.ProspectiveRentPercentage.Replace("%", string.Empty), CultureInfo.InvariantCulture) / 100,
            sharedFacilities = (int)invln_Facilities.Selfcontainedfacilities,
            targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(homeTypesData.General.Exceeds80PercentOfMarketRent),
            whichNDSSStandardsHaveBeenMet = { (int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths },
        };
        await _homeTypeCrmContext.Save(disabledHomeTypeDto, loginData.OrganisationId, CancellationToken.None);
        await _homeTypeCrmContext.Save(generalHomeTypeDto, loginData.OrganisationId, CancellationToken.None);
    }

    private async Task AddDeliveryPhases(ILoginData loginData, string applicationId, DeliveryPhasesData deliveryPhasesData)
    {
        var firstDeliveryPhase = new DeliveryPhaseDto()
        {
            acquisitionDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            acquisitionPaymentDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            applicationId = applicationId,
            completionDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            completionPaymentDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            isCompleted = true,
            isReconfigurationOfExistingProperties = deliveryPhasesData.RehabDeliveryPhase.ReconfiguringExisting,
            name = deliveryPhasesData.RehabDeliveryPhase.Name,
            numberOfHomes =
                new Dictionary<string, int?>
                {
                    {
                        deliveryPhasesData.RehabDeliveryPhase.DeliveryPhaseHomes[0].Id,
                        deliveryPhasesData.RehabDeliveryPhase.DeliveryPhaseHomes[0].NumberOfHomes
                    },
                },
            rehabBuildActivityType = (int)invln_RehabActivityType.WorksOnly,
            startOnSiteDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            startOnSitePaymentDate = new DateTime(
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            typeOfHomes = "rehab",
        };
        var secondDeliveryPhase = new DeliveryPhaseDto()
        {
            applicationId = applicationId,
            completionDate = new DateTime(
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestoneDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestoneDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestoneDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            completionPaymentDate = new DateTime(
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestonePaymentDate.Day!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestonePaymentDate.Month!, CultureInfo.InvariantCulture),
                int.Parse(deliveryPhasesData.OffTheShelfDeliveryPhase.CompletionMilestonePaymentDate.Year!, CultureInfo.InvariantCulture),
                0,
                0,
                0,
                DateTimeKind.Unspecified),
            isCompleted = true,
            name = deliveryPhasesData.OffTheShelfDeliveryPhase.Name,
            newBuildActivityType = (int)invln_NewBuildActivityType.OffTheShelf,
            numberOfHomes = new Dictionary<string, int?>
            {
                {
                    deliveryPhasesData.OffTheShelfDeliveryPhase.DeliveryPhaseHomes[0].Id,
                    deliveryPhasesData.OffTheShelfDeliveryPhase.DeliveryPhaseHomes[0].NumberOfHomes
                },
            },
            typeOfHomes = "newBuild",
        };

        await _deliveryPhaseCrmContext.Save(firstDeliveryPhase, loginData.OrganisationId, CancellationToken.None);
        await _deliveryPhaseCrmContext.Save(secondDeliveryPhase, loginData.OrganisationId, CancellationToken.None);
    }
}
