extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.PrefillData.Data;
using HE.Investment.AHP.Domain.PrefillData.Repositories;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using MediatR;
using SiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteQueryHandler : IRequestHandler<GetSiteQuery, SiteModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    private readonly IAhpPrefillDataRepository _prefillDataRepository;

    public GetSiteQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository, IAhpPrefillDataRepository prefillDataRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _prefillDataRepository = prefillDataRepository;
    }

    public async Task<SiteModel> Handle(GetSiteQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(new SiteId(request.SiteId), userAccount, cancellationToken);
        var prefillData = site.FrontDoorProjectId.IsProvided() && site.FrontDoorSiteId.IsProvided()
            ? await _prefillDataRepository.GetAhpSitePrefillData(site.FrontDoorProjectId!, site.FrontDoorSiteId!, cancellationToken)
            : null;
        var localAuthority = LocalAuthorityMapper.Map(site.LocalAuthority);

        return new SiteModel
        {
            Id = site.Id.Value,
            Name = site.Name.Value,
            Status = site.Status,
            Section106 = CreateSection106(site),
            LocalAuthority = localAuthority,
            PlanningDetails = CreateSitePlanningDetails(site.PlanningDetails, localAuthority?.Code, prefillData),
            NationalDesignGuidePriorities = site.NationalDesignGuidePriorities.Values.ToList(),
            BuildingForHealthyLife = site.BuildingForHealthyLife,
            NumberOfGreenLights = site.NumberOfGreenLights?.ToString(),
            LandAcquisitionStatus = site.LandAcquisitionStatus.Value,
            TenderingStatusDetails = CreateSiteTenderingStatusDetails(site.TenderingStatusDetails),
            StrategicSiteDetails = CreateStrategicSiteDetails(site.StrategicSiteDetails),
            SiteTypeDetails = CreateSiteTypeDetails(site.SiteTypeDetails),
            SiteUseDetails = CreateSiteUseDetails(site.SiteUseDetails),
            RuralClassification = CreateSiteRuralClassification(site.RuralClassification),
            EnvironmentalImpact = site.EnvironmentalImpact?.Value,
            SiteProcurements = site.Procurements.Procurements.ToList(),
            ModernMethodsOfConstruction = CreateSiteModernMethodsOfConstruction(site.ModernMethodsOfConstruction),
        };
    }

    private static Section106Dto CreateSection106(SiteEntity site)
    {
        return new Section106Dto(
            site.Id.ToString(),
            site.Name.Value,
            site.Section106.GeneralAgreement,
            site.Section106.AffordableHousing,
            site.Section106.OnlyAffordableHousing,
            site.Section106.AdditionalAffordableHousing,
            site.Section106.CapitalFundingEligibility,
            site.Section106.LocalAuthorityConfirmation,
            site.Section106.IsIneligible(),
            site.Section106.IsIneligibleDueToAffordableHousing(),
            site.Section106.IsIneligibleDueToCapitalFundingGuide());
    }

    private static SitePlanningDetails CreateSitePlanningDetails(
        PlanningDetails planningDetails,
        string? localAuthorityCode,
        AhpSitePrefillData? prefillData)
    {
        return new SitePlanningDetails(
            planningDetails.PlanningStatus ?? prefillData?.PlanningStatus,
            planningDetails.ReferenceNumber?.Value,
            planningDetails.IsQuestionActive(nameof(planningDetails.ReferenceNumber)),
            DateValueObjectMapper.ToContract(planningDetails.DetailedPlanningApprovalDate),
            planningDetails.IsQuestionActive(nameof(planningDetails.DetailedPlanningApprovalDate)),
            planningDetails.RequiredFurtherSteps?.Value,
            planningDetails.IsQuestionActive(nameof(planningDetails.RequiredFurtherSteps)),
            DateValueObjectMapper.ToContract(planningDetails.ApplicationForDetailedPlanningSubmittedDate),
            planningDetails.IsQuestionActive(nameof(planningDetails.ApplicationForDetailedPlanningSubmittedDate)),
            DateValueObjectMapper.ToContract(planningDetails.ExpectedPlanningApprovalDate),
            planningDetails.IsQuestionActive(nameof(planningDetails.ExpectedPlanningApprovalDate)),
            DateValueObjectMapper.ToContract(planningDetails.OutlinePlanningApprovalDate),
            planningDetails.IsQuestionActive(nameof(planningDetails.OutlinePlanningApprovalDate)),
            planningDetails.IsGrantFundingForAllHomesCoveredByApplication,
            planningDetails.IsQuestionActive(nameof(planningDetails.IsGrantFundingForAllHomesCoveredByApplication)),
            DateValueObjectMapper.ToContract(planningDetails.PlanningSubmissionDate),
            planningDetails.IsQuestionActive(nameof(planningDetails.PlanningSubmissionDate)),
            planningDetails.LandRegistryDetails?.IsLandRegistryTitleNumberRegistered,
            planningDetails.LandRegistryDetails?.TitleNumber?.Value,
            planningDetails.LandRegistryDetails?.IsGrantFundingForAllHomesCoveredByTitleNumber,
            planningDetails.IsQuestionActive(nameof(planningDetails.LandRegistryDetails)),
            planningDetails.IsAnswered(),
            localAuthorityCode);
    }

    private static SiteTenderingStatusDetails CreateSiteTenderingStatusDetails(TenderingStatusDetails tenderingStatusDetails)
    {
        return new SiteTenderingStatusDetails(
            tenderingStatusDetails.TenderingStatus,
            tenderingStatusDetails.ContractorName?.Value,
            tenderingStatusDetails.IsSmeContractor,
            tenderingStatusDetails.IsIntentionToWorkWithSme);
    }

    private static StrategicSite CreateStrategicSiteDetails(StrategicSiteDetails? details)
    {
        return new StrategicSite(
            details?.IsStrategicSite,
            details?.SiteName?.Value);
    }

    private static SiteTypeDetails CreateSiteTypeDetails(ValueObjects.SiteTypeDetails details)
    {
        return new SiteTypeDetails(
            details.SiteType,
            details.IsOnGreenBelt,
            details.IsRegenerationSite,
            details.IsAnswered());
    }

    private static SiteUseDetails CreateSiteUseDetails(ValueObjects.SiteUseDetails details)
    {
        return new SiteUseDetails(details.IsPartOfStreetFrontInfill, details.IsForTravellerPitchSite, details.TravellerPitchSiteType);
    }

    private static SiteRuralClassification CreateSiteRuralClassification(ValueObjects.SiteRuralClassification details)
    {
        return new SiteRuralClassification(details.IsWithinRuralSettlement, details.IsRuralExceptionSite);
    }

    private static SiteModernMethodsOfConstruction CreateSiteModernMethodsOfConstruction(ValueObjects.Mmc.SiteModernMethodsOfConstruction mmc)
    {
        return new SiteModernMethodsOfConstruction(
            mmc.SiteUsingModernMethodsOfConstruction,
            mmc.ModernMethodsOfConstruction?.ModernMethodsConstructionCategories.ToList(),
            mmc.ModernMethodsOfConstruction?.ModernMethodsConstruction2DSubcategories.ToList(),
            mmc.ModernMethodsOfConstruction?.ModernMethodsConstruction3DSubcategories.ToList(),
            mmc.FutureAdoption?.Plans?.Value,
            mmc.FutureAdoption?.ExpectedImpact?.Value,
            mmc.Information?.Barriers?.Value,
            mmc.Information?.Impact?.Value);
    }
}
