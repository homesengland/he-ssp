using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideHomesCountCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomesCountCommand, OperationResult>
{
    public ProvideHomesCountCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomesCountCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideHomesCount(new HomesCount(request.HomesCount)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
