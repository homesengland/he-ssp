using System.Text.Json;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.Xrm.Sdk;

namespace HE.InvestmentLoans.BusinessLogic.User.Repositories;

public class LoanUserRepository : ILoanUserRepository
{
    private readonly IOrganizationService _serviceClient;
    private readonly IRedisService _redisService;

    public LoanUserRepository(IOrganizationService serviceClient, IRedisService redisService)
    {
        _serviceClient = serviceClient;
        _redisService = redisService;
    }

    public ContactRolesDto? GetUserDetails(string userGlobalId, string userEmail)
    {
        var portalType = "858110001";

        var redisKey = $"loanUser_{portalType}_{userGlobalId}_{userEmail}";

        if (_redisService.Exists(redisKey))
        {
            return _redisService.ObjectGet<ContactRolesDto>(redisKey);
        }

        var req = new invln_getcontactroleRequest()
        {
            invln_contactemail = userEmail,
            invln_contactexternalid = userGlobalId,
            invln_portaltype = portalType,
        };

        var resp = (invln_getcontactroleResponse)_serviceClient.Execute(req);
        if (resp.invln_portalroles != null)
        {
            var roles = JsonSerializer.Deserialize<ContactRolesDto>(resp.invln_portalroles);

            if (roles != null)
            {
                _redisService.ObjectSet(redisKey, roles, 30);
            }

            return roles;
        }

        return null;
    }
}
