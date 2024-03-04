using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;

public class ProvidePrivateSectorFundingCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvidePrivateSectorFundingCommand, OperationResult>
{
    public ProvidePrivateSectorFundingCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext)
        : base(fundingRepository, loanApplicationRepository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvidePrivateSectorFundingCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var privateSectorFunding = request.IsApplied.IsProvided() ? PrivateSectorFunding.FromString(request.IsApplied!, request.PrivateSectorFundingApplyResult, request.PrivateSectorFundingNotApplyingReason) : null;

                x.ProvidePrivateSectorFunding(privateSectorFunding);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
