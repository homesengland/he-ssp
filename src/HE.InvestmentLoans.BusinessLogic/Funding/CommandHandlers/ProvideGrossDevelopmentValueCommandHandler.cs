using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class ProvideGrossDevelopmentValueCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideGrossDevelopmentValueCommand, OperationResult>
{
    public ProvideGrossDevelopmentValueCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
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
