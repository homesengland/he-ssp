using HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities;
using HE.Investment.AHP.BusinessLogic.FinancialDetails.Repositories;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.BusinessLogic.FinancialDetails.CommandHandlers;
public class FinancialDetailsCommandHandlerBase
{
    private readonly IFinancialDetailsRepository _repository;

    //private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<FinancialDetailsCommandHandlerBase> _logger;

    public FinancialDetailsCommandHandlerBase(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult> Perform(Action<FinancialDetailsEntity> action, FinancialDetailsId financialDetailsId, CancellationToken cancellationToken)
    {
        //var userAccount = await _loanUserContext.GetSelectedAccount();

        var financialScheme = await _repository.GetById(financialDetailsId, cancellationToken)
            ?? throw new NotFoundException(nameof(FinancialDetails), financialDetailsId);


        try
        {
            action(financialScheme);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(financialScheme, cancellationToken);
        return OperationResult.Success();
    }
}
