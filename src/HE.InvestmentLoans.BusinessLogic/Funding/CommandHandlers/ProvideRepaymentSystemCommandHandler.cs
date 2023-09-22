using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;

public class ProvideRepaymentSystemCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideRepaymentSystemCommand, OperationResult>
{
    public ProvideRepaymentSystemCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
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
