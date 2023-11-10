using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideAffordableHomesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideAffordableHomesCommand, OperationResult>
{
    public ProvideAffordableHomesCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideAffordableHomesCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideAffordableHomes(new AffordableHomes(request.AffordableHomes)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
