using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class CheckProjectAnswersCommandHandler : ProjectCommandHandlerBase, IRequestHandler<CheckProjectAnswersCommand, OperationResult>
{
    public CheckProjectAnswersCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(CheckProjectAnswersCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer()),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
