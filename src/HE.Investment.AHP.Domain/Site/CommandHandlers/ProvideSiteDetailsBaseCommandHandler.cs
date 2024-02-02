using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public abstract class ProvideSiteDetailsBaseCommandHandler<TCommand> : SiteBaseCommandHandler, IRequestHandler<TCommand, OperationResult>
    where TCommand : IProvideSiteDetailsCommand, IRequest<OperationResult>
{
    protected ProvideSiteDetailsBaseCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                Provide(request, site);

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }

    protected abstract void Provide(TCommand request, SiteEntity site);
}
