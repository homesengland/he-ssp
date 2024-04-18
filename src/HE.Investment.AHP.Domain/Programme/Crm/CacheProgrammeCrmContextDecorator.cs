extern alias Org;

using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Programme.Crm;

internal class CacheProgrammeCrmContextDecorator : IProgrammeCrmContext
{
    private readonly ICacheService _cache;

    private readonly IProgrammeCrmContext _decorated;

    public CacheProgrammeCrmContextDecorator(IProgrammeCrmContext decorated, ICacheService cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        return (await _cache.GetValueAsync($"ahp-programme-{programmeId}", async () => await _decorated.GetProgramme(programmeId, cancellationToken)))!;
    }
}
