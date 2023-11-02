using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideLandOwnershipCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandOwnershipCommand, OperationResult>
{
    public ProvideLandOwnershipCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandOwnershipCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails => financialDetails.ProvideLandOwnership(new LandOwnership(request.LandOwnership)),
            request.FinancialDetailsId,
            cancellationToken);
    }
}
