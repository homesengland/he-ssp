using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Domain.User.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Account.Domain.User.CommandHandlers;

public class ProvideAcceptHeTermsAndConditionsCommandHandler : IRequestHandler<ProvideAcceptHeTermsAndConditionsCommand, OperationResult>
{
    private readonly ILogger<ProvideAcceptHeTermsAndConditionsCommandHandler> _logger;

    public ProvideAcceptHeTermsAndConditionsCommandHandler(ILogger<ProvideAcceptHeTermsAndConditionsCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OperationResult> Handle(ProvideAcceptHeTermsAndConditionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            AcceptHeTermsAndConditions.FromString(request.HeTermsAndConditions);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        return await Task.FromResult(OperationResult.Success());
    }
}
