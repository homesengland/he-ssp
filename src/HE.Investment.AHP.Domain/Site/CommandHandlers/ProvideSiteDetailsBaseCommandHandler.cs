using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public abstract class ProvideSiteDetailsBaseCommandHandler<TCommand> : SiteBaseCommandHandler, IRequestHandler<TCommand, OperationResult>
    where TCommand : IProvideSiteDetailsCommand, IRequest<OperationResult>
{
    protected ProvideSiteDetailsBaseCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            async site => await Provide(request, site, cancellationToken),
            request.SiteId,
            cancellationToken);
    }

    protected virtual void Provide(TCommand request, SiteEntity site)
    {
    }

    protected virtual Task Provide(TCommand request, SiteEntity site, CancellationToken cancellationToken)
    {
        Provide(request, site);
        return Task.CompletedTask;
    }
}
