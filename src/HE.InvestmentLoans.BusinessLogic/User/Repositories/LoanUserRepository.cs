using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.ValueObjects;
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

    public async Task<ContactRolesDto?> GetUserAccount(UserGlobalId userGlobalId, string userEmail)
    {
        const string portalType = "858110001";

        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId.ToString(),
            invln_portaltype = portalType,
        };

        var resp_async = await _serviceClient.ExecuteAsync(req);
        var resp = resp_async != null ? (invln_getcontactroleResponse)resp_async : throw new NotFoundException("Contact role", userGlobalId);
        if (resp.invln_portalroles != null)
        {
            return CrmResponseSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);
        }

        return null;
    }
}
