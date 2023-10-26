using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideAffordableHomesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideAffordableHomesCommand, OperationResult>
{
    public ProvideAffordableHomesCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
