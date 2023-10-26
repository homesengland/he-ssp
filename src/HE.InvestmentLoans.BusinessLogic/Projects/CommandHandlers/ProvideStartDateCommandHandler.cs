using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideStartDateCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideStartDateCommand, OperationResult>
{
    public ProvideStartDateCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideStartDateCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.Exists.IsNotProvided())
                {
                    return;
                }

                project.ProvideStartDate(StartDate.From(request.Exists, request.Year, request.Month, request.Day));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
