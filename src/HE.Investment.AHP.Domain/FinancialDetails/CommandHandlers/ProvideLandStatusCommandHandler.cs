using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class ProvideLandStatusCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandStatusCommand, OperationResult>
{
    public ProvideLandStatusCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandStatusCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var purchasePrice = request.IsFinal && request.PurchasePrice.IsProvided() ? PurchasePrice.From(request.PurchasePrice!) : null;
                var expectedPurchasePrice = !request.IsFinal && request.PurchasePrice.IsProvided() ? ExpectedPurchasePrice.From(request.PurchasePrice!) : null;
                financialDetails.ProvideLandStatus(purchasePrice, expectedPurchasePrice);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
