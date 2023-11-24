using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideGrantFundingInformationCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideGrantFundingInformationCommand, OperationResult>
{
    public ProvideGrantFundingInformationCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrantFundingInformationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideGrantFundingInformation(PublicSectorGrantFunding.FromString(
                request.ProviderName,
                request.Amount,
                request.Name,
                request.Purpose)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
