using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Projects.Commands;
using HE.Investments.Loans.Contract.Projects.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

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
