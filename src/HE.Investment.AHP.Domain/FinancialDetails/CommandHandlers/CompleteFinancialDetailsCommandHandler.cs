using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public class CompleteFinancialDetailsCommandHandler : FinancialDetailsCommandHandlerBase, IRequestHandler<CompleteFinancialDetailsCommand, OperationResult>
{
    public CompleteFinancialDetailsCommandHandler(IFinancialDetailsRepository repository, IAccountUserContext accountUserContext, ILogger<FinancialDetailsCommandHandlerBase> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(CompleteFinancialDetailsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            financialDetails => financialDetails.CompleteFinancialDetails(request.IsSectionCompleted),
            request.ApplicationId,
            cancellationToken);
    }
}
