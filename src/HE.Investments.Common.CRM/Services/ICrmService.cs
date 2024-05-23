using Microsoft.Xrm.Sdk;

namespace HE.Investments.Common.CRM.Services;

public interface ICrmService
{
    Task<TDto> ExecuteAsync<TRequest, TResponse, TDto>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse;

    Task<string> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse;

    Task<TResult> ExecuteAsync<TRequest, TResponse, TResult>(TRequest request, Func<TResponse, TResult> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse;
}
