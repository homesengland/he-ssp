using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvidePlanningPermissionStatusCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvidePlanningPermissionStatusCommand, OperationResult>
{
    public ProvidePlanningPermissionStatusCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvidePlanningPermissionStatusCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                var status = request.PlanningPermissionStatus switch
                {
                    ProjectFormOption.PlanningPermissionStatusOptions.ReceivedFull => PlanningPermissionStatus.ReceivedFull,
                    ProjectFormOption.PlanningPermissionStatusOptions.NotReceived => PlanningPermissionStatus.NotReceived,
                    ProjectFormOption.PlanningPermissionStatusOptions.NotSubmitted => PlanningPermissionStatus.NotSubmitted,
                    ProjectFormOption.PlanningPermissionStatusOptions.OutlineOrConsent => PlanningPermissionStatus.OutlineOrConsent,
                    _ => (PlanningPermissionStatus?)null!,
                };

                project.ProvidePlanningPermissionStatus(status);
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
