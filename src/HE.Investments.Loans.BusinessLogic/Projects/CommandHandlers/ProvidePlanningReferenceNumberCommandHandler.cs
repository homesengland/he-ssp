using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvidePlanningReferenceNumberCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvidePlanningReferenceNumberCommand, OperationResult>
{
    public ProvidePlanningReferenceNumberCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvidePlanningReferenceNumberCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.Exists.IsNotProvided())
                {
                    return;
                }

                project.ProvidePlanningReferenceNumber(PlanningReferenceNumber.FromString(request.Exists, request.PlanningReferenceNumber));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
