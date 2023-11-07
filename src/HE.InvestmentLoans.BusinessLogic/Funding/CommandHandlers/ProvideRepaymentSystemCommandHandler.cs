using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideRepaymentSystemCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideRepaymentSystemCommand, OperationResult>
{
    public ProvideRepaymentSystemCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
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
