using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideHomesCountCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomesCountCommand, OperationResult>
{
    public ProvideHomesCountCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomesCountCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideHomesCount(HomesCount.From(request.HomesCount)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
