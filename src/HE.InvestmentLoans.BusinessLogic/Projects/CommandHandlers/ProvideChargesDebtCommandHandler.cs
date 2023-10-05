using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                if(request.ChargesDebtInfo.IsNotProvided())
                {
                    return;
                }

                // TODO
                project.ProvideChargesDebt(new ChargesDebt(request.ChargesDebt), new ChargesDebtInfo(request.ChargesDebtInfo));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
