using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class FundingBaseCommandHandler
{
    private readonly IFundingRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public FundingBaseCommandHandler(IFundingRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(Action<FundingEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var funding = await _repository.GetAsync(loanApplicationId, userAccount, cancellationToken);

        try
        {
            action(funding);
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(funding, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
