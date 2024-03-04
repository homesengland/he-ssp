using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLandOwnershipCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLandOwnershipCommand, OperationResult>
{
    public ProvideLandOwnershipCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
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
