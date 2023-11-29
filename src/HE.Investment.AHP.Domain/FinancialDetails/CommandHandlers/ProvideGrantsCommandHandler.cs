using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideGrantsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideGrantsCommand, OperationResult>
{
    public ProvideGrantsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrantsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var aggregatedResults = OperationResult.New();

                var countyCouncilGrants = request.CountyCouncilGrants.IsProvided() ? aggregatedResults.CatchResult(() => new CountyCouncilGrants(request.CountyCouncilGrants ?? string.Empty)) : null;
                var dHSCExtraCareGrants = request.DHSCExtraCareGrants.IsProvided() ? aggregatedResults.CatchResult(() => new DHSCExtraCareGrants(request.DHSCExtraCareGrants ?? string.Empty)) : null;
                var localAuthorityGrants = request.LocalAuthorityGrants.IsProvided() ? aggregatedResults.CatchResult(() => new LocalAuthorityGrants(request.LocalAuthorityGrants ?? string.Empty)) : null;
                var socialServicesGrants = request.SocialServicesGrants.IsProvided() ? aggregatedResults.CatchResult(() => new SocialServicesGrants(request.SocialServicesGrants ?? string.Empty)) : null;
                var healthRelatedGrants = request.HealthRelatedGrants.IsProvided() ? aggregatedResults.CatchResult(() => new HealthRelatedGrants(request.HealthRelatedGrants ?? string.Empty)) : null;
                var lotteryGrants = request.LotteryGrants.IsProvided() ? aggregatedResults.CatchResult(() => new LotteryGrants(request.LotteryGrants ?? string.Empty)) : null;
                var otherPublicGrants = request.OtherPublicBodiesGrants.IsProvided() ? aggregatedResults.CatchResult(() => new OtherPublicGrants(request.OtherPublicBodiesGrants ?? string.Empty)) : null;

                aggregatedResults.CheckErrors();

                financialDetails.ProvideGrants(
                    countyCouncilGrants,
                    dHSCExtraCareGrants,
                    localAuthorityGrants,
                    socialServicesGrants,
                    healthRelatedGrants,
                    lotteryGrants,
                    otherPublicGrants);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
