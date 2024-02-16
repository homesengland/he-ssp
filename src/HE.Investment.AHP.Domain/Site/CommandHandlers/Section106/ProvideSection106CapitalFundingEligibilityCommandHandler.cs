using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.Section106;

public class ProvideSection106CapitalFundingEligibilityCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideSection106CapitalFundingEligibilityCommand, OperationResult>
{
    public ProvideSection106CapitalFundingEligibilityCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    public Task<OperationResult> Handle(ProvideSection106CapitalFundingEligibilityCommand request, CancellationToken cancellationToken)
    {
        return Perform(
            site =>
            {
                site.ProvideSection106(site.Section106.WithCapitalFundingEligibility(request.CapitalFundingEligibility));
                return Task.FromResult(OperationResult.Success());
            },
            request.SiteId,
            cancellationToken);
    }
}
