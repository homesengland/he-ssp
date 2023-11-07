using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandler : FundingBaseCommandHandler, IRequestHandler<CheckAnswersFundingSectionCommand, OperationResult>
{
    public CheckAnswersFundingSectionCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(CheckAnswersFundingSectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(x => x.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer()), request.LoanApplicationId, cancellationToken);
    }
}
