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

public class ProvideLandStatusCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandStatusCommand, OperationResult>
{
    public ProvideLandStatusCommandHandler(IFinancialDetailsRepository repository, IAccountUserContext accountUserContext, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandStatusCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                var purchasePrice = request.IsFinal && request.PurchasePrice.IsProvided() ? new PurchasePrice(request.PurchasePrice!) : null;
                var expectedPurchasePrice = !request.IsFinal && request.PurchasePrice.IsProvided() ? new ExpectedPurchasePrice(request.PurchasePrice!) : null;
                var landStatus = new LandStatus(purchasePrice, expectedPurchasePrice);
                financialDetails.ProvideLandStatus(landStatus);
            },
            request.ApplicationId,
            cancellationToken);
    }
}
