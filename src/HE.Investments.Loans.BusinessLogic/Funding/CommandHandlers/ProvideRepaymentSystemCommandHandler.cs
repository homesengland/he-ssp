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

public class ProvideRepaymentSystemCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideRepaymentSystemCommand, OperationResult>
{
    public ProvideRepaymentSystemCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(fundingRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideRepaymentSystemCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var repaymentSystem = request.RefinanceOrRepay.IsProvided() ? RepaymentSystem.New(request.RefinanceOrRepay!, request.AdditionalInformation) : null;

                x.ProvideRepaymentSystem(repaymentSystem);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
