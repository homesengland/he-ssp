using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;
public class FinancialDetailsCommandHandlerBase
{
    private readonly IFinancialDetailsRepository _repository;

    private readonly ILogger<FinancialDetailsCommandHandlerBase> _logger;

    public FinancialDetailsCommandHandlerBase(IFinancialDetailsRepository repository, ILogger<FinancialDetailsCommandHandlerBase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OperationResult> Perform(Action<FinancialDetailsEntity> action, ApplicationId applicationId, CancellationToken cancellationToken)
    {
        var financialDetails = await _repository.GetById(applicationId, cancellationToken);

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
