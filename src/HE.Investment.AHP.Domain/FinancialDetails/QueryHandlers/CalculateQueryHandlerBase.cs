using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.QueryHandlers;

public abstract class CalculateQueryHandlerBase
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    private readonly ILogger _logger;

    protected CalculateQueryHandlerBase(IFinancialDetailsRepository financialDetailsRepository, IConsortiumUserContext accountUserContext, ILogger logger)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _accountUserContext = accountUserContext;
        _logger = logger;
    }

    protected async Task<(OperationResult OperationResult, CalculationResult CalculationResult)> Perform(Func<FinancialDetailsEntity, CalculationResult> action, AhpApplicationId applicationId, CancellationToken cancellationToken)
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
