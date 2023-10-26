using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain;

public class DomainExceptionHandler : IDomainExceptionHandler
{
    private readonly ILogger _logger;

    public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OperationResult<TResult>> Handle<TResult>(Func<Task<OperationResult<TResult>>> action)
    {
        try
        {
            return await action();
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return new OperationResult<TResult>(domainValidationException.OperationResult.Errors, default!);
        }
    }
}
