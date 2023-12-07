using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public abstract class BaseQueryHandler
{
    private readonly ILogger _logger;

    protected BaseQueryHandler(ILogger logger)
    {
        _logger = logger;
    }

    protected IList<ErrorItem> PerformWithValidation(params Action[] actions)
    {
        var errors = new List<ErrorItem>();
        foreach (var action in actions)
        {
            try
            {
                action();
            }
            catch (DomainValidationException domainValidationException)
            {
                _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
                errors.AddRange(domainValidationException.OperationResult.Errors);
            }
        }

        return errors;
    }
}
