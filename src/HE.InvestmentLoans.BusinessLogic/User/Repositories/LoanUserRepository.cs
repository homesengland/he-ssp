using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public class LoanUserRepository : ILoanUserRepository
{
    private readonly IOrganizationService _serviceClient;

    public LoanUserRepository(IOrganizationService serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public ContactRolesDto? GetUserDetails(string userGlobalId, string userEmail)
    {
        var portalType = "858110001";

        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId,
            invln_portaltype = portalType,
        };

        var resp = (invln_getcontactroleResponse)_serviceClient.Execute(req);
        if (resp.invln_portalroles != null)
        {
            return JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
        }

        return null;
    }
}
