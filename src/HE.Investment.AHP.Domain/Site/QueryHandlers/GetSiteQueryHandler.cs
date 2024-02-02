using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;
using MediatR;

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
            Section106GeneralAgreement = site.Section106?.GeneralAgreement,
            Section106AffordableHousing = site.Section106?.AffordableHousing,
            Section106OnlyAffordableHousing = site.Section106?.OnlyAffordableHousing,
            Section106AdditionalAffordableHousing = site.Section106?.AdditionalAffordableHousing,
            Section106CapitalFundingEligibility = site.Section106?.CapitalFundingEligibility,
            Section106LocalAuthorityConfirmation = site.Section106?.LocalAuthorityConfirmation,
            IsIneligibleDueToAffordableHousing = site.Section106?.IsIneligibleDueToAffordableHousing(),
            IsIneligibleDueToCapitalFundingGuide = site.Section106?.IsIneligibleDueToCapitalFundingGuide(),
            IsIneligible = site.Section106?.IsIneligible(),
            LocalAuthority = LocalAuthorityMapper.Map(site.LocalAuthority),
            PlanningDetails = CreateSitePlanningDetails(site.PlanningDetails),
            TenderingStatusDetails = CreateSiteTenderingStatusDetails(site.TenderingStatusDetails),
        };
    }

    private SitePlanningDetails CreateSitePlanningDetails(PlanningDetails planningDetails)
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

    private SiteTenderingStatusDetails CreateSiteTenderingStatusDetails(TenderingStatusDetails tenderingStatusDetails)
    {
        return new SiteTenderingStatusDetails(
            tenderingStatusDetails.TenderingStatus,
            tenderingStatusDetails.ContractorName?.Value,
            tenderingStatusDetails.IsSmeContractor);
    }
}
