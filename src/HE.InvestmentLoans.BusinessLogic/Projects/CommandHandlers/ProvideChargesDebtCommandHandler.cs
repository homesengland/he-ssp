using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideChargesDebtCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideChargesDebtCommand, OperationResult>
{
    public ProvideChargesDebtCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
