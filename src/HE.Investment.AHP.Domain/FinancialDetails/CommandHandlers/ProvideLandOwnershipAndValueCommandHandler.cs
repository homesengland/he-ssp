using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideLandOwnershipAndValueCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandOwnershipAndValueCommand, OperationResult>
{
    public ProvideLandOwnershipAndValueCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandOwnershipAndValueCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails => financialDetails.ProvideLandOwnershipAndValue(new LandOwnership(request.LandOwnership), new LandValue(request.LandValue)),
            request.ApplicationId,
            cancellationToken);
    }
}
