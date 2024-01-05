using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public abstract class CalculateQueryHandlerBase
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ILogger _logger;

    protected CalculateQueryHandlerBase(IFinancialDetailsRepository financialDetailsRepository, IAccountUserContext accountUserContext, ILogger logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _accountUserContext = accountUserContext;
        _logger = logger;
    }

    protected async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Perform(Func<FinancialDetailsEntity, CalculationResult> action, ApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var financialDetails = await _financialDetailsRepository.GetById(applicationId, account, cancellationToken);
        CalculationResult calculationResult;

        try
        {
            calculationResult = action(financialDetails);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return (domainValidationException.OperationResult, new CalculationResult(null, null));
        }

        return (OperationResult.Success(), calculationResult);
    }
}
