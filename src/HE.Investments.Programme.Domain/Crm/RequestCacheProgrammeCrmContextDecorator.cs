using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.Programme.Domain.Crm;

internal sealed class RequestCacheProgrammeCrmContextDecorator : IProgrammeCrmContext
{
    private readonly InMemoryCache<ProgrammeDto, string> _cache = new();

    private readonly IProgrammeCrmContext _decorated;

    public RequestCacheProgrammeCrmContextDecorator(IProgrammeCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(programmeId.ToGuidAsString(), async () => await _decorated.GetProgramme(programmeId, cancellationToken)))!;
    }

    public async Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken)
    {
        return await _decorated.GetProgrammes(cancellationToken);
    }
}
