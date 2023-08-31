using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureBaseCommandHandler
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public CompanyStructureBaseCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(Action<CompanyStructureEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(loanApplicationId, userAccount, cancellationToken);

        try
        {
            action(companyStructure);
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
