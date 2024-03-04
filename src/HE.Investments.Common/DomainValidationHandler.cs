using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Common;

public class DomainValidationHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : DomainValidationException
    where TResponse : IOperationResult, new()
{
    private readonly ILogger<DomainValidationHandler<TRequest, TResponse, TException>> _logger;

    public DomainValidationHandler(
        ILogger<DomainValidationHandler<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        _logger.LogWarning(exception, "Validation error(s) occured: {Message}", exception.Message);

        var result = new TResponse();
        result.AddValidationErrors(exception.OperationResult.Errors);

        state.SetHandled(result);

        return Task.CompletedTask;
    }
}
