using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureBaseCommandHandler
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<CompanyStructureBaseCommandHandler> _logger;

    public CompanyStructureBaseCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(Func<CompanyStructureEntity, Task> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(loanApplicationId, userAccount, CompanyStructureFieldsSet.GetAllFields, cancellationToken);

        try
        {
            await action(companyStructure);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
