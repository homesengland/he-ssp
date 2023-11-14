using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLocalAuthorityCommand, OperationResult>
{
    public ProvideLocalAuthorityCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLocalAuthorityCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.LocalAuthorityId.IsNotProvided() || request.LocalAuthorityName.IsNotProvided())
                {
                    project.ProvideLocalAuthority(null);
                }
                else
                {
                    project.ProvideLocalAuthority(LocalAuthority.New(request.LocalAuthorityId!, request.LocalAuthorityName!));
                }
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
