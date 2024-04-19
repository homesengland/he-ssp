using HE.Investment.AHP.Contract.PrefillData;
using HE.Investment.AHP.Contract.PrefillData.Queries;
using HE.Investment.AHP.Domain.PrefillData.Repositories;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.PrefillData.QueryHandlers;

public class GetAhpSitePrefillDataQueryHandler : IRequestHandler<GetAhpSitePrefillDataQuery, AhpSitePrefillData>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAhpPrefillDataRepository _prefillDataRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetAhpSitePrefillDataQueryHandler(ISiteRepository siteRepository, IAhpPrefillDataRepository prefillDataRepository, IAccountUserContext accountUserContext)
    {
        _siteRepository = siteRepository;
        _prefillDataRepository = prefillDataRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<AhpSitePrefillData> Handle(GetAhpSitePrefillDataQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSiteBasicInfo(request.SiteId, userAccount, cancellationToken);

        if (site.FrontDoorProjectId.IsProvided() && site.FrontDoorSiteId.IsProvided())
        {
            var prefillData = await _prefillDataRepository.GetAhpSitePrefillData(site.FrontDoorProjectId!, site.FrontDoorSiteId!, cancellationToken);
            return new AhpSitePrefillData(prefillData.LocalAuthorityName);
        }

        return AhpSitePrefillData.Empty;
    }
}
