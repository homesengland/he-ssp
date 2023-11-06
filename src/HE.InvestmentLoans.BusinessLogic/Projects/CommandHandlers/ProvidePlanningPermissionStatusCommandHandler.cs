using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvidePlanningPermissionStatusCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvidePlanningPermissionStatusCommand, OperationResult>
{
    public ProvidePlanningPermissionStatusCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
