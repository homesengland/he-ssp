using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;

public class ProvideAcceptHeTermsAndConditionsCommandHandler : IRequestHandler<ProvideAcceptHeTermsAndConditionsCommand, OperationResult>
{
    private readonly ILogger<ProvideUserDetailsCommandHandler> _logger;

    public ProvideAcceptHeTermsAndConditionsCommandHandler(ILogger<ProvideUserDetailsCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OperationResult> Handle(ProvideAcceptHeTermsAndConditionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(() => AcceptHeTermsAndConditions.FromString(request.HeTermsAndConditions), cancellationToken);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        return OperationResult.Success();
    }
}
