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
public class ChangeProjectNameCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ChangeProjectNameCommand, OperationResult>
{
    public ChangeProjectNameCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
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
