using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
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
            financialDetails =>
            {
                if (request.LandOwnership.IsProvided())
                {
                    financialDetails.ProvideLandOwnership(new LandOwnership(request.LandOwnership));
                }

                if (request.LandValue.IsProvided())
                {
                    financialDetails.ProvideLandValue(new LandValue(request.LandValue));
                }
            },
            request.ApplicationId,
            cancellationToken);
    }
}
