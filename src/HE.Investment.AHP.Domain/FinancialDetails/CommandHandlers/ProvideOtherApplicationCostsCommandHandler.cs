using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideOtherApplicationCostsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideOtherApplicationCostsCommand, OperationResult>
{
    public ProvideOtherApplicationCostsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideOtherApplicationCostsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var aggregatedResults = OperationResult.New();

                var expectedWorksCost = request.ExpectedWorksCosts.IsProvided() ? aggregatedResults.CatchResult(() => new ExpectedWorksCosts(request.ExpectedWorksCosts)) : null;
                var expectedOnCosts = request.ExpectedOnCosts.IsProvided() ? aggregatedResults.CatchResult(() => new ExpectedOnCosts(request.ExpectedOnCosts)) : null;

                aggregatedResults.CheckErrors();

                financialDetails.ProvideExpectedCosts(expectedWorksCost, expectedOnCosts);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
