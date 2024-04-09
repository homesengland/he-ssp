using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investments.AHP.IntegrationTests.Crm;

public class AhpApplicationCrmContext
{
    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public AhpApplicationCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task ChangeApplicationStatus(
        string applicationId,
        Guid organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        CancellationToken cancellationToken)
    {
        var crmStatus = AhpApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeahpapplicationstatusRequest
        {
            invln_applicationid = applicationId,
            invln_organisationid = organisationId.ToString(),
            invln_userid = _userContext.UserGlobalId,
            invln_newapplicationstatus = crmStatus,
            invln_changereason = $"[IntegrationTests] {changeReason}",
        };

        await _service.ExecuteAsync<invln_changeahpapplicationstatusRequest, invln_changeahpapplicationstatusResponse>(
            request,
            r => r.ResponseName,
            cancellationToken);
    }
}
