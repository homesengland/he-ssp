using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ChangeProjectNameCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ChangeProjectNameCommand, OperationResult>
{
    public ChangeProjectNameCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ChangeProjectNameCommand request, CancellationToken cancellationToken)
    {
        if (request.Name.IsNotProvided())
        {
            return OperationResult.Success();
        }

        return await Perform(
            project => project.ChangeName(new ProjectName(request.Name)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
