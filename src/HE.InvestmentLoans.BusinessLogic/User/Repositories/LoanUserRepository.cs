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
        var portalType = "858110001";

        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId,
            invln_portaltype = portalType,
        };

        var resp = (invln_getcontactroleResponse) await _serviceClient.ExecuteAsync(req);
        if (resp.invln_portalroles != null)
        {
            return JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
        }

        return null;
    }
}
