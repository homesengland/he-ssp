using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideHomesCountCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomesCountCommand, OperationResult>
{
    public ProvideHomesCountCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
