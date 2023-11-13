using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Account.Domain.Utils;

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
        // TODO exception
        var response = await _serviceClient.ExecuteAsync(request, cancellationToken) as TResponse
                       ?? throw new NotFoundException("a");

        return CrmResponseSerializer.Deserialize<TDto>(getResponse(response))
               ?? throw new NotFoundException("b");
    }
}
