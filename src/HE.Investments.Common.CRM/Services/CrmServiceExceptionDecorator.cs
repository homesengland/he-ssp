using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Exceptions;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Common.CRM.Services;

internal sealed class CrmServiceExceptionDecorator : ICrmService
{
    private readonly ICrmService _decorated;

    public CrmServiceExceptionDecorator(ICrmService decorated)
    {
        _decorated = decorated;
    }

    public async Task<TDto> ExecuteAsync<TRequest, TResponse, TDto>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse
    {
        return await HandleException<TRequest, TDto>(async () =>
            await _decorated.ExecuteAsync<TRequest, TResponse, TDto>(request, getResponse, cancellationToken));
    }

    public async Task<string> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse
    {
        return await HandleException<TRequest, string>(async () => await _decorated.ExecuteAsync(request, getResponse, cancellationToken));
    }

    public async Task<TResult> ExecuteAsync<TRequest, TResponse, TResult>(TRequest request, Func<TResponse, TResult> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse
    {
        return await HandleException<TRequest, TResult>(async () => await _decorated.ExecuteAsync(request, getResponse, cancellationToken));
    }

    private static async Task<TResult> HandleException<TRequest, TResult>(Func<Task<TResult>> crmRequest)
    {
        try
        {
            return await crmRequest();
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            throw new CrmException($"CRM exception when executing request {typeof(TRequest).Name} {ex.Message}", ex);
        }
    }
}
