using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideNameCommandHandler : IRequestHandler<ProvideNameCommand, OperationResult<SiteId>>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public ProvideNameCommandHandler(ISiteRepository siteRepository, IConsortiumUserContext consortiumUserContext)
    {
        _siteRepository = siteRepository;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<OperationResult<SiteId>> Handle(ProvideNameCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        var site = request.SiteId.IsProvided()
            ? await _siteRepository.GetSite(request.SiteId!, userAccount, cancellationToken)
            : SiteEntity.NewSite(userAccount, request.FrontDoorProjectId, request.FrontDoorSiteId);
        var siteName = new SiteName(request.Name);

        await site.ProvideName(siteName, _siteRepository, cancellationToken);

        var siteId = await _siteRepository.Save(site, userAccount, cancellationToken);
        return OperationResult.Success(siteId);
    }
}
