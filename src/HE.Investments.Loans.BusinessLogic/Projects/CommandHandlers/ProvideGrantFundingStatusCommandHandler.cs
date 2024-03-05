using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

internal class ProvideGrantFundingStatusCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideGrantFundingStatusCommand, OperationResult>
{
    public ProvideGrantFundingStatusCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
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
