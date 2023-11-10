using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
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

                if (request.ExpectedWorksCosts.IsProvided())
                {
                    var expectedWorksCost = aggregatedResults.CatchResult(() => new ExpectedWorksCosts(request.ExpectedWorksCosts));
                    financialDetails.ProvideExpectedWorksCosts(expectedWorksCost);
                }

                if (request.ExpectedOnCosts.IsProvided())
                {
                    var expectedOnCosts = aggregatedResults.CatchResult(() => new ExpectedOnCosts(request.ExpectedOnCosts));
                    financialDetails.ProvideExpectedOnCosts(expectedOnCosts);
                }

                aggregatedResults.CheckErrors();
            },
            request.ApplicationId,
            cancellationToken);
    }
}
