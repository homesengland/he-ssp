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

public class ProvideNationalDesignGuidePrioritiesCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideNationalDesignGuidePrioritiesCommand, OperationResult>
{
    public ProvideNationalDesignGuidePrioritiesCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public Task<OperationResult> Handle(ProvideNationalDesignGuidePrioritiesCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                if (request.NationalDesignGuidePriorities.IsProvided())
                {
                    var designPriorities = new NationalDesignGuidePriorities(request.NationalDesignGuidePriorities);
                    site.ProvideNationalDesignGuidePriorities(designPriorities);
                }

                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
