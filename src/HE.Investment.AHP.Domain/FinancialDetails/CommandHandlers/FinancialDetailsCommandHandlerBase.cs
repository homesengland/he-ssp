using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.BusinessLogic.FinancialDetails.CommandHandlers;
public class FinancialDetailsCommandHandlerBase
{
    private readonly IFinancialDetailsRepository _repository;

    private readonly ILogger<FinancialDetailsCommandHandlerBase> _logger;

    public FinancialDetailsCommandHandlerBase(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult> Perform(Action<FinancialDetailsEntity> action, FinancialDetailsId financialDetailsId, CancellationToken cancellationToken)
    {
        var financialDetails = await _repository.GetById(financialDetailsId, cancellationToken);

        try
        {
            action(financialDetails);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(financialDetails, cancellationToken);
        return OperationResult.Success();
    }
}
