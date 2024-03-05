using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideAffordableHomesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideAffordableHomesCommand, OperationResult>
{
    public ProvideAffordableHomesCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
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
