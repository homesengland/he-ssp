using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Security.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class CheckSecurityAnswersCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ConfirmSecuritySectionCommand, OperationResult>
{
    public CheckSecurityAnswersCommandHandler(ISecurityRepository securityRepository, ILoanUserContext userContext, ILogger<SecurityBaseCommandHandler> logger)
        : base(securityRepository, userContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ConfirmSecuritySectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(security => security.CheckAnswers(request.Answer.ToYesNoAnswer()), request.Id, cancellationToken);
    }
}
