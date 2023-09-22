using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class ProvideGrossDevelopmentValueCommandHandler : FundingBaseCommandHandler, IRequestHandler<ProvideGrossDevelopmentValueCommand, OperationResult>
{
    public ProvideGrossDevelopmentValueCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
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
