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

public class ProvideChargesDebtCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideChargesDebtCommand, OperationResult>
{
    public ProvideChargesDebtCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideChargesDebtCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.ChargesDebt.IsNotProvided())
                {
                    return;
                }

                project.ProvideChargesDebt(ChargesDebt.From(request.ChargesDebt, request.ChargesDebtInfo));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
