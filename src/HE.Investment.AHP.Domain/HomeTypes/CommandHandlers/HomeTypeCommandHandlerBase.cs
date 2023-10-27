using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public abstract class HomeTypeCommandHandlerBase
{
    private readonly ILogger _logger;

    protected HomeTypeCommandHandlerBase(ILogger logger)
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
                return domainValidationException.OperationResult.Errors;
            }
        }

        return errors;
    }
}
