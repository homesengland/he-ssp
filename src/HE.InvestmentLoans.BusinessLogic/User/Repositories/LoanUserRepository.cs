using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public class LoanUserRepository : ILoanUserRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public LoanUserRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<ContactRolesDto?> GetUserDetails(string userGlobalId, string userEmail)
    {
        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId,
            invln_portaltype = "858110001",
        };

        var resp_async = await _serviceClient.ExecuteAsync(req);
        var resp = resp_async != null ? (invln_getcontactroleResponse)resp_async : throw new Exception("invln_getcontactroleResponse exception");
        if (resp.invln_portalroles != null)
        {
            return JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
        }

        return null;
    }
}
