extern alias Org;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Cache;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Programme.Crm;

internal class RequestCacheProgrammeCrmContextDecorator : IProgrammeCrmContext
{
    private readonly InMemoryCache<ProgrammeDto, string> _cache = new();

    private readonly IProgrammeCrmContext _decorated;

    public RequestCacheProgrammeCrmContextDecorator(IProgrammeCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(ShortGuid.ToGuidAsString(programmeId), async () => await _decorated.GetProgramme(programmeId, cancellationToken)))!;
    }
}
