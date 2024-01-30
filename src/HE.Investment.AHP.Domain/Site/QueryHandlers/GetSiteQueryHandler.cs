using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investment.AHP.Domain.Site.Repositories;
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
            PlanningDetails = site.PlanningDetails != null ? new SitePlanningDetails(site.PlanningDetails.PlanningStatus) : null,
            LocalAuthority = LocalAuthorityMapper.Map(site.LocalAuthority),
        };
    }
}
