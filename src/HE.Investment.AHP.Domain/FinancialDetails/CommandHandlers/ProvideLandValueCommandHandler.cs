using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class ProvideLandValueCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<ProvideLandValueCommand, OperationResult>
{
    public ProvideLandValueCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandValueCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails =>
            {
                if (request.LandOwnership.IsProvided() || request.LandValue.IsProvided())
                {
                    financialDetails.ProvideLandValue(new LandValue(request.LandOwnership, request.LandValue));
                }
            },
            request.ApplicationId,
            cancellationToken);
    }
}
