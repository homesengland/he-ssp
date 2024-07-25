using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investments.AHP.IntegrationTests.Crm;
using HE.Investments.AHP.IntegrationTests.Order02FillSite.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.DeliveryPhases;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data.HomeTypes;
using HE.Investments.AHP.IntegrationTests.Order05ManageAllocation.Data.Allocation;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Utils;

public class AhpDataManipulator
{
    private readonly AhpCrmContext _ahpCrmContext;

    public AhpDataManipulator(AhpCrmContext ahpCrmContext)
    {
        _ahpCrmContext = ahpCrmContext;
    }

    public async Task<string> CreateAhpAllocation(
        ILoginData loginData,
        SiteData siteData,
        FinancialDetailsData financialDetailsData,
        SchemeInformationData schemeInformationData,
        HomeTypesData homeTypesData,
        DeliveryPhasesData deliveryPhasesData,
        AllocationData allocationData)
    {
        if (string.IsNullOrEmpty(siteData.SiteId))
        {
            siteData.GenerateSiteName();
            var siteId = await CreateSite(loginData, siteData);
            siteData.SetSiteId(siteId);
        }

        var allocationId = await CreateAllocation(loginData, siteData, financialDetailsData, schemeInformationData, allocationData.AllocationName);
        await AddHomeTypes(loginData, allocationId, homeTypesData, schemeInformationData);
        await AddDeliveryPhases(loginData, allocationId, deliveryPhasesData, homeTypesData, schemeInformationData);

        return allocationId;
    }

    private async Task<string> CreateSite(ILoginData loginData, SiteData siteData)
    {
        var dto = new SiteDto
        {
            name = siteData.SiteName,
            localAuthority = new SiteLocalAuthority
            {
                id = siteData.LocalAuthorityCode,
                name = siteData.LocalAuthorityName,
            },
            status = (int)invln_Sitestatus.InProgress,
        };

        return await _ahpCrmContext.SaveAhpSite(dto, loginData, CancellationToken.None);
    }

    private async Task<string> CreateAllocation(
        ILoginData loginData,
        SiteData siteData,
        FinancialDetailsData financialDetailsData,
        SchemeInformationData schemeInformationData,
        string allocationName)
    {
        var applicationDto = new AhpApplicationDto
        {
            siteId = ShortGuid.TryToGuidAsString(siteData.SiteId),
            applicationPartnerConfirmation = true,
            applicationStatus = (int)invln_scheme_StatusCode.Approved,
            borrowingAgainstRentalIncomeFromThisScheme = financialDetailsData.ExpectedContributionsRentalIncomeBorrowing,
            contactExternalId = loginData.UserGlobalId,
            currentLandValue = financialDetailsData.PublicLandValue,
            dateSubmitted = DateTime.UtcNow,
            deliveryPhasesSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            developingPartnerId = loginData.OrganisationId,
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
            name = allocationName,
            noOfHomes = schemeInformationData.HousesToDeliver,
            organisationId = loginData.OrganisationId,
            otherCapitalSources = financialDetailsData.ExpectedContributionsOtherCapitalSources,
            ownResources = financialDetailsData.ExpectedContributionsOwnResources,
            ownerOfTheHomesAfterCompletionId = loginData.OrganisationId,
            ownerOfTheLandDuringDevelopmentId = loginData.OrganisationId,
            previousExternalStatus = (int)invln_scheme_StatusCode.ApplicationSubmitted,
            programmeId = "d5fe3baa-eeae-ee11-a569-0022480041cf",
            recycledCapitalGrantFund = financialDetailsData.ExpectedContributionsRcgfContribution,
            representationsandwarranties = true,
            schemeInformationSectionCompletionStatus = (int)invln_Sectioncompletionstatus.Completed,
            tenure = (int)invln_Tenure.Affordablerent,
            isAllocation = true,
        };
        var applicationId = await _ahpCrmContext.SaveAhpApplication(applicationDto, loginData, CancellationToken.None);

        return applicationId;
    }

    private async Task AddHomeTypes(ILoginData loginData, string applicationId, HomeTypesData homeTypesData, SchemeInformationData schemeInformationData)
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
            designPrinciples = [(int)invln_HAPPIprinciples.KNone],
            doAllHomesMeetNDSS = false,
            exemptionJustification = homeTypesData.Disabled.ExemptionJustification,
            exitPlan = homeTypesData.Disabled.ExitPlan,
            floorArea = homeTypesData.Disabled.FloorArea,
            fundingSources = [(int)invln_Revenuefundingsources.SocialServicesDepartment],
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
            [
                (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements
            ],
            mmcCategories1Subcategories = [(int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout],
            mmcCategories2Subcategories =
            [
                (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows
            ],
            moveOnArrangements = homeTypesData.Disabled.MoveOnArrangements,
            numberOfBedrooms = homeTypesData.Disabled.NumberOfBedrooms,
            numberOfHomes = schemeInformationData.HousesToDeliver / 2,
            numberOfStoreys = homeTypesData.Disabled.NumberOfStoreys,
            prospectiveRent = homeTypesData.Disabled.RentPerWeek,
            prospectiveRentAsPercentOfMarketRent =
                int.Parse(homeTypesData.Disabled.ProspectiveRentPercentage.Replace("%", string.Empty), CultureInfo.InvariantCulture) / 100,
            revenueFunding = (int)invln_Revenuefunding.Yesrevenuefundingisneededandhasbeenidentified,
            sharedFacilities = (int)invln_Facilities.Selfcontainedfacilities,
            shortStayAccommodation = YesNoTypeMapper.Map(homeTypesData.Disabled.ShortStayAccommodation),
            targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(homeTypesData.Disabled.Exceeds80PercentOfMarketRent),
            typologyLocationAndDesign = homeTypesData.Disabled.TypologyLocationAndDesign,
            whichNDSSStandardsHaveBeenMet = [(int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths],
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
            [
                (int)invln_MMCCategories.Category1Premanufacturing3Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category2Premanufacturing2Dprimarystructuralsystems,
                (int)invln_MMCCategories.Category6Traditionalbuildingproductledsitelabourreductionproductivityimprovements
            ],
            mmcCategories1Subcategories = [(int)invln_MMCcategory1subcategories._1astructuralchassisonlynotfittedout],
            mmcCategories2Subcategories =
            [
                (int)invln_MMCcategory2subcategories._2cfurtherenhancedconsolidationinsulationliningsexternalcladdingroofingdoorswindows
            ],
            numberOfBedrooms = homeTypesData.General.NumberOfBedrooms,
            numberOfHomes = schemeInformationData.HousesToDeliver / 2,
            numberOfStoreys = homeTypesData.General.NumberOfStoreys,
            prospectiveRent = homeTypesData.General.RentPerWeek,
            prospectiveRentAsPercentOfMarketRent =
                int.Parse(homeTypesData.General.ProspectiveRentPercentage.Replace("%", string.Empty), CultureInfo.InvariantCulture) / 100,
            sharedFacilities = (int)invln_Facilities.Selfcontainedfacilities,
            targetRentOver80PercentOfMarketRent = YesNoTypeMapper.Map(homeTypesData.General.Exceeds80PercentOfMarketRent),
            whichNDSSStandardsHaveBeenMet = [(int)invln_WhichNDSSstandardshavebeenmet.Bedroomwidths],
        };

        var disabledHomeTypeId = await _ahpCrmContext.SaveAhpHomeType(disabledHomeTypeDto, loginData, CancellationToken.None);
        var generalHomeTypeId = await _ahpCrmContext.SaveAhpHomeType(generalHomeTypeDto, loginData, CancellationToken.None);

        homeTypesData.Disabled.SetHomeTypeId(disabledHomeTypeId);
        homeTypesData.General.SetHomeTypeId(generalHomeTypeId);
    }

    private async Task AddDeliveryPhases(ILoginData loginData, string applicationId, DeliveryPhasesData deliveryPhasesData, HomeTypesData homeTypesData, SchemeInformationData schemeInformationData)
    {
        var rehabDeliveryPhase = new DeliveryPhaseDto()
        {
            acquisitionDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestoneDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            acquisitionPaymentDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.AcquisitionMilestonePaymentDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            applicationId = applicationId,
            completionDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            completionPaymentDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            isCompleted = true,
            isReconfigurationOfExistingProperties = deliveryPhasesData.RehabDeliveryPhase.ReconfiguringExisting,
            name = deliveryPhasesData.RehabDeliveryPhase.Name,
            numberOfHomes = [],
            rehabBuildActivityType = (int)invln_RehabActivityType.WorksOnly,
            startOnSiteDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestoneDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            startOnSitePaymentDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.StartOnSiteMilestonePaymentDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            typeOfHomes = "rehab",
        };
        var offTheShelfDeliveryPhase = new DeliveryPhaseDto()
        {
            name = deliveryPhasesData.OffTheShelfDeliveryPhase.Name,
            applicationId = applicationId,
            numberOfHomes = [],
            completionDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestoneDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            completionPaymentDate = DateTime.ParseExact(
                $"{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Day}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Month}/{deliveryPhasesData.RehabDeliveryPhase.CompletionMilestonePaymentDate.Year}",
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture),
            isCompleted = true,
            newBuildActivityType = (int)invln_NewBuildActivityType.OffTheShelf,
            typeOfHomes = "newBuild",
        };
        var rehabDeliveryPhaseId = await _ahpCrmContext.SaveAhpDeliveryPhase(rehabDeliveryPhase, loginData, CancellationToken.None);

        var offTheShelfDeliveryPhaseId = await _ahpCrmContext.SaveAhpDeliveryPhase(offTheShelfDeliveryPhase, loginData, CancellationToken.None);

        rehabDeliveryPhase.id = rehabDeliveryPhaseId;
        rehabDeliveryPhase.numberOfHomes =
            new Dictionary<string, int?>
            {
                { homeTypesData.Disabled.Id, schemeInformationData.HousesToDeliver / 2 },
            };
        offTheShelfDeliveryPhase.id = offTheShelfDeliveryPhaseId;
        offTheShelfDeliveryPhase.numberOfHomes = new Dictionary<string, int?>
        {
            { homeTypesData.General.Id, schemeInformationData.HousesToDeliver / 2 },
        };

        await _ahpCrmContext.SaveAhpDeliveryPhase(rehabDeliveryPhase, loginData, CancellationToken.None);
        await _ahpCrmContext.SaveAhpDeliveryPhase(offTheShelfDeliveryPhase, loginData, CancellationToken.None);
        deliveryPhasesData.RehabDeliveryPhase.SetDeliveryPhaseId(rehabDeliveryPhaseId);
    }
}
