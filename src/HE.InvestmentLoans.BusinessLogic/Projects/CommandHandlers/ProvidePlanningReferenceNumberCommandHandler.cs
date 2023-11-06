using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvidePlanningReferenceNumberCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvidePlanningReferenceNumberCommand, OperationResult>
{
    public ProvidePlanningReferenceNumberCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
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
