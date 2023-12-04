using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class CompleteFinancialDetailsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<CompleteFinancialDetailsCommand, OperationResult>
{
    public CompleteFinancialDetailsCommandHandler(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, logger)
    {
    }

    public async Task<OperationResult> Handle(CompleteFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails => financialDetails.CompleteFinancialDetails(),
            request.ApplicationId,
            cancellationToken);
    }
}
