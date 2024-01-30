using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideSitePlanningStatusCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSitePlanningStatusCommand, OperationResult>
{
    public ProvideSitePlanningStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public Task<OperationResult> Handle(ProvideSitePlanningStatusCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                var planningDetails = new PlanningDetails(request.SitePlanningStatus);

                site.ProvidePlanningDetails(planningDetails);
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
