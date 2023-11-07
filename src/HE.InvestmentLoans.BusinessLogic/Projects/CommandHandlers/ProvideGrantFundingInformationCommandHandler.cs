using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideGrantFundingInformationCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideGrantFundingInformationCommand, OperationResult>
{
    public ProvideGrantFundingInformationCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrantFundingInformationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project => project.ProvideGrantFundingInformation(PublicSectorGrantFunding.FromString(request.ProviderName, request.Amount, request.Name, request.Purpose)),
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
