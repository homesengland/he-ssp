using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
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
            financialDetails => financialDetails.ProvideExpectedCosts(new ExpectedCosts(request.ExpectedWorksCosts, request.ExpectedOnCosts)),
            request.ApplicationId,
            cancellationToken);
    }
}
