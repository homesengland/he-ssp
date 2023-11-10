using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideHomesTypesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomesTypesCommand, OperationResult>
{
    public ProvideHomesTypesCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomesTypesCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.HomesTypes.IsNotProvided())
                {
                    return;
                }

                project.ProvideHomesTypes(new HomesTypes(request.HomesTypes, request.OtherHomesTypes));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
