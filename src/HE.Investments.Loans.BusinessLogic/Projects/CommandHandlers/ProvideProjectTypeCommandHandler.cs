using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideProjectTypeCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideProjectTypeCommand, OperationResult>
{
    public ProvideProjectTypeCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
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
