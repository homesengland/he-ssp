using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.TenderingStatus;

public class ProvideIsIntentionToWorkWithSmeCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideIsIntentionToWorkWithSmeCommand>
{
    public ProvideIsIntentionToWorkWithSmeCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideIsIntentionToWorkWithSmeCommand request, SiteEntity site)
    {
        site.ProvideTenderingStatusDetails(new TenderingStatusDetails(site.TenderingStatusDetails.TenderingStatus, request.IsIntentionToWorkWithSme));
    }
}
