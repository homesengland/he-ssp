using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Extensions;
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
                financialDetails.ProvideCurrentLandValue(request.LandValue.IsProvided() ? new CurrentLandValue(request.LandValue!) : null);
                financialDetails.ProvideIsPublicLand(request.LandOwnership.MapToBool());
            },
            request.ApplicationId,
            cancellationToken);
    }
}
