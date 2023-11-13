using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

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
