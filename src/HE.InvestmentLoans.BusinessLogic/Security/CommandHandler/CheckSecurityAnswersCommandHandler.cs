using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Security.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class CheckSecurityAnswersCommandHandler : SecurityBaseCommandHandler, IRequestHandler<ConfirmSecuritySectionCommand, OperationResult>
{
    public CheckSecurityAnswersCommandHandler(ISecurityRepository securityRepository, ILoanUserContext userContext)
        : base(securityRepository, userContext)
    {
    }

    public async Task<OperationResult> Handle(ConfirmSecuritySectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(security => security.CheckAnswers(request.Answer.ToYesNoAnswer()), request.Id, cancellationToken);
    }
}
