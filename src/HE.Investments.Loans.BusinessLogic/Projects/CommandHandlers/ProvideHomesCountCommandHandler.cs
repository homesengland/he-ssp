using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

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
