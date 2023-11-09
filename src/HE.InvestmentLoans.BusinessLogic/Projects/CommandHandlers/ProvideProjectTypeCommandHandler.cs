using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideProjectTypeCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideProjectTypeCommand, OperationResult>
{
    public ProvideProjectTypeCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideProjectTypeCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideProjectType(new ProjectType(request.ProjectType)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
