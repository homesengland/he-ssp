using Microsoft.Xrm.Sdk;

namespace HE.Investments.Account.Domain.Utils;

public interface ICrmService
{
    Task<TDto> ExecuteAsync<TRequest, TResponse, TDto>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse;
}
