using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideRepaymentSystemCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideRepaymentSystemCommand, OperationResult>
{
    public ProvideRepaymentSystemCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
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
