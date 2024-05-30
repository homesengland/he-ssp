using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.Programme.Domain.Crm;

public class ProgrammeCrmContext : IProgrammeCrmContext
{
    private readonly ICrmService _service;

    public ProgrammeCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        var request = new invln_getsingleprogrammeRequest
        {
            invln_programmeId = programmeId,
        };

        return await _service.ExecuteAsync<invln_getsingleprogrammeRequest, invln_getsingleprogrammeResponse, ProgrammeDto>(
            request,
            r => r.invln_programme,
            cancellationToken);
    }

    public async Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultipleprogrammeRequest, invln_getmultipleprogrammeResponse, IList<ProgrammeDto>>(
            new invln_getmultipleprogrammeRequest(),
            r => r.invln_programmeList,
            cancellationToken);
    }
}
