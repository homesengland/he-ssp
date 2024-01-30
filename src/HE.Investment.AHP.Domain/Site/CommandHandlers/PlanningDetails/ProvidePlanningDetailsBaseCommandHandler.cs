using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.PlanningDetails;

public abstract class ProvidePlanningDetailsBaseCommandHandler<TCommand> : SiteBaseCommandHandler, IRequestHandler<TCommand, OperationResult>
    where TCommand : IProvideSitePlanningDetailsCommand, IRequest<OperationResult>
{
    protected ProvidePlanningDetailsBaseCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
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
