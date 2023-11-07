using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideLandOwnershipCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLandOwnershipCommand, OperationResult>
{
    public ProvideLandOwnershipCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
