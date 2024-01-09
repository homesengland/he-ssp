using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideOtherApplicationCostsCommandHandler : FinancialDetailsCommandHandlerBase,
    IRequestHandler<ProvideOtherApplicationCostsCommand, OperationResult>
{
    public ProvideOtherApplicationCostsCommandHandler(IFinancialDetailsRepository repository, IAccountUserContext accountUserContext, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideOtherApplicationCostsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var result = OperationResult.New();
                var expectedWorksCosts = request.ExpectedWorksCosts.IsProvided() ? result.CatchResult(() => new ExpectedWorksCosts(request.ExpectedWorksCosts!)) : null;
                var expectedOnCosts = request.ExpectedOnCosts.IsProvided() ? result.CatchResult(() => new ExpectedOnCosts(request.ExpectedOnCosts!)) : null;
                result.CheckErrors();

                financialDetails.ProvideOtherApplicationCosts(new OtherApplicationCosts(expectedWorksCosts, expectedOnCosts));
            },
            request.ApplicationId,
            cancellationToken);
    }
}
