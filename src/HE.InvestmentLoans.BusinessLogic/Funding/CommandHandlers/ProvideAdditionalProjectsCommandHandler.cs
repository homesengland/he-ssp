using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideAdditionalProjectsCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideAdditionalProjectsCommand, OperationResult>
{
    public ProvideAdditionalProjectsCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<FundingBaseCommandHandler> logger)
        : base(fundingRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideAdditionalProjectsCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var additionalProjects = request.IsThereAnyAdditionalProject.IsProvided()
                    ? AdditionalProjects.FromString(request.IsThereAnyAdditionalProject!)
                    : null;

                x.ProvideAdditionalProjects(additionalProjects);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
