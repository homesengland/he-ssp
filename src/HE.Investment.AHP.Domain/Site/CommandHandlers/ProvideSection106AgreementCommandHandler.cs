using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSection106AgreementCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106AgreementCommand, OperationResult>
{
    public ProvideSection106AgreementCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideSection106AgreementCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async site =>
            {
                var agreement = new SiteSection106Agreement(request.Agreement);
                await site.ProvideSection106Agreement(agreement, cancellationToken);
            },
            new SiteId(request.SiteId!),
            cancellationToken);
    }
}
