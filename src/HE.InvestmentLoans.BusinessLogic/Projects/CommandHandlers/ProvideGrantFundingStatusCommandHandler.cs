using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

internal class ProvideGrantFundingStatusCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideGrantFundingStatusCommand, OperationResult>
{
    public ProvideGrantFundingStatusCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrantFundingStatusCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                var status = GrantFundingStatusMapper.FromString(request.Status);

                if (status is null)
                {
                    return;
                }

                project.ProvideGrantFundingStatus(status.Value);
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
