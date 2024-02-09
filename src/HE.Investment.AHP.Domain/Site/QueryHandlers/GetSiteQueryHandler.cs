using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;
using MediatR;
using SiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteQueryHandler : IRequestHandler<GetSiteQuery, SiteModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    public GetSiteQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<SiteModel> Handle(GetSiteQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(new SiteId(request.SiteId), userAccount, cancellationToken);

        return new SiteModel
        {
            Id = site.Id.Value,
            Name = site.Name.Value,
            Section106 = CreateSection106(site),
            LocalAuthority = LocalAuthorityMapper.Map(site.LocalAuthority),
            PlanningDetails = CreateSitePlanningDetails(site.PlanningDetails),
            NationalDesignGuidePriorities = site.NationalDesignGuidePriorities.Values.ToList(),
            BuildingForHealthyLife = site.BuildingForHealthyLife,
            NumberOfGreenLights = site.NumberOfGreenLights?.ToString(),
            TenderingStatusDetails = CreateSiteTenderingStatusDetails(site.TenderingStatusDetails),
            StrategicSiteDetails = CreateStrategicSiteDetails(site.StrategicSiteDetails),
            SiteTypeDetails = CreateSiteTypeDetails(site.SiteTypeDetails),
            SiteUseDetails = CreateSiteUseDetails(site.SiteUseDetails),
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

    private static SitePlanningDetails CreateSitePlanningDetails(PlanningDetails planningDetails)
    {
        return new SitePlanningDetails(
            planningDetails.PlanningStatus,
            planningDetails.ReferenceNumber?.Value,
            DateValueObjectMapper.ToContract(planningDetails.DetailedPlanningApprovalDate),
            planningDetails.RequiredFurtherSteps?.Value,
            DateValueObjectMapper.ToContract(planningDetails.ApplicationForDetailedPlanningSubmittedDate),
            DateValueObjectMapper.ToContract(planningDetails.ExpectedPlanningApprovalDate),
            DateValueObjectMapper.ToContract(planningDetails.OutlinePlanningApprovalDate),
            planningDetails.IsGrantFundingForAllHomesCoveredByApplication,
            DateValueObjectMapper.ToContract(planningDetails.PlanningSubmissionDate),
            planningDetails.LandRegistryDetails?.IsLandRegistryTitleNumberRegistered,
            planningDetails.LandRegistryDetails?.TitleNumber?.Value,
            planningDetails.LandRegistryDetails?.IsGrantFundingForAllHomesCoveredByTitleNumber,
            planningDetails.IsAnswered());
    }

    private static SiteTenderingStatusDetails CreateSiteTenderingStatusDetails(TenderingStatusDetails tenderingStatusDetails)
    {
        return new SiteTenderingStatusDetails(
            tenderingStatusDetails.TenderingStatus,
            tenderingStatusDetails.ContractorName?.Value,
            tenderingStatusDetails.IsSmeContractor,
            tenderingStatusDetails.IsIntentionToWorkWithSme);
    }

    private static StrategicSite CreateStrategicSiteDetails(StrategicSiteDetails details)
    {
        return new StrategicSite(
            details.IsStrategicSite,
            details.SiteName?.Value);
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
}
