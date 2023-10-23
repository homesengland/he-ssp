using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ChangeProjectNameCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ChangeProjectNameCommand, OperationResult>
{
    public ChangeProjectNameCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
