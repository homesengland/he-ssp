using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLandOwnershipCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLandOwnershipCommand, OperationResult>
{
    public ProvideLandOwnershipCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLandOwnershipCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.ApplicantHasFullOwnership.IsNotProvided())
                {
                    return;
                }

                project.ProvideLandOwnership(LandOwnership.From(request.ApplicantHasFullOwnership));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
