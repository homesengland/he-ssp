using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Validation;
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
            financialDetails => financialDetails.ProvidePurchasePrice(new PurchasePrice(request.PurchasePrice)),
            request.FinancialDetailsId,
            cancellationToken);
    }
}
