using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
public class ProvideGrossDevelopmentValueCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideGrossDevelopmentValueCommand, OperationResult>
{
    public ProvideGrossDevelopmentValueCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(fundingRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideGrossDevelopmentValueCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var grossDevelopmentValue = request.GrossDevelopmentValue.IsProvided() ? GrossDevelopmentValue.FromString(request.GrossDevelopmentValue!) : null;
                x.ProvideGrossDevelopmentValue(grossDevelopmentValue);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
