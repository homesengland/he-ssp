extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Domain.Programme.Crm;

public class ProgrammeCrmContext : IProgrammeCrmContext
{
    private readonly ICrmService _service;

    public ProgrammeCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleprogrammeRequest();

        var response = await _service.ExecuteAsync<invln_getmultipleprogrammeRequest, invln_getmultipleprogrammeResponse, IList<ProgrammeDto>>(
            request,
            r => r.invln_programmeList,
            cancellationToken);

        return response;
    }
}
