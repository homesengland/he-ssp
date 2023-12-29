using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Exceptions;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Common.CRM.Services;

public class CrmService : ICrmService
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public CrmService(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<TDto> ExecuteAsync<TRequest, TResponse, TDto>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse
    {
        var response = await _serviceClient.ExecuteAsync(request, cancellationToken) as TResponse
                       ?? throw new NotFoundException($"Cannot find {nameof(TDto)} resource for {nameof(TRequest)} request");

        return CrmResponseSerializer.Deserialize<TDto>(getResponse(response))
               ?? throw new NotFoundException($"Cannot find {nameof(TDto)} resource for {nameof(TRequest)} request");
    }

    public async Task<string> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<TResponse, string> getResponse, CancellationToken cancellationToken)
        where TRequest : OrganizationRequest
        where TResponse : OrganizationResponse
    {
        var response = await _serviceClient.ExecuteAsync(request, cancellationToken) as TResponse
                       ?? throw new NotFoundException($"Cannot find resource for {nameof(TRequest)} request");

        return getResponse(response);
    }
}
