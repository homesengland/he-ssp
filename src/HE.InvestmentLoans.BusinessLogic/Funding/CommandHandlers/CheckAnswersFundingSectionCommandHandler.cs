using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Funding.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandler : FundingBaseCommandHandler, IRequestHandler<CheckAnswersFundingSectionCommand, OperationResult>
{
    public CheckAnswersFundingSectionCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(CheckAnswersFundingSectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(x => x.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer()), request.LoanApplicationId, cancellationToken);
    }
}
