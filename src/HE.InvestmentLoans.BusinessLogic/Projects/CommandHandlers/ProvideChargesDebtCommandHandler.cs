using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideChargesDebtCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideChargesDebtCommand, OperationResult>
{
    public ProvideChargesDebtCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
