using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;

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
