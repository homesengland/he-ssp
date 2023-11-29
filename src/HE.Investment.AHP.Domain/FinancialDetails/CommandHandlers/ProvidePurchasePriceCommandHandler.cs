using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvidePurchasePriceCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvidePurchasePriceCommand, OperationResult>
{
    public ProvidePurchasePriceCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvidePurchasePriceCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                ActualPurchasePrice? actualPurchasePrice = null;
                ExpectedPurchasePrice? expectedPurchasePrice = null;
                if (request.ActualPurchasePrice.IsProvided())
                {
                    actualPurchasePrice = new ActualPurchasePrice(request.ActualPurchasePrice);
                }

                if (request.ExpectedPurchasePrice.IsProvided())
                {
                    expectedPurchasePrice = new ExpectedPurchasePrice(request.ExpectedPurchasePrice);
                }

                financialDetails.ProvidePurchasePrice(actualPurchasePrice, expectedPurchasePrice);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
