using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class CompleteSiteCommandHandler : ProvideSiteDetailsBaseCommandHandler<CompleteSiteCommand>
{
    public CompleteSiteCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(CompleteSiteCommand request, SiteEntity site)
    {
        site.Complete(request.IsSectionCompleted);
    }
}
