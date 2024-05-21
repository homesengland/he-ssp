extern alias Org;

using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Consortium.Domain.Crm;

public class RequestCacheConsortiumCrmContextDecorator : IConsortiumCrmContext
{
    private readonly InMemoryCache<ConsortiumDto, string> _cache = new();

    private readonly IConsortiumCrmContext _decorated;

    public RequestCacheConsortiumCrmContextDecorator(IConsortiumCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<ConsortiumDto> GetConsortium(string consortiumId, string organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            consortiumId.ToGuidAsString(),
            async () => await _decorated.GetConsortium(consortiumId, organisationId, cancellationToken)))!;
    }

    public async Task<IList<ConsortiumDto>> GetConsortiumsListByMemberId(string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetConsortiumsListByMemberId(organisationId, cancellationToken);
    }

    public async Task<string> CreateConsortium(string userId, string programmeId, string consortiumName, string leadOrganisationId, CancellationToken cancellationToken)
    {
        return await _decorated.CreateConsortium(userId, programmeId, consortiumName, leadOrganisationId, cancellationToken);
    }

    public async Task<bool> IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsConsortiumExistForProgrammeAndOrganisation(programmeId, organisationId, cancellationToken);
    }

    public async Task CreateJoinConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        _cache.Delete(consortiumId.ToGuidAsString());
        await _decorated.CreateJoinConsortiumRequest(consortiumId, organisationId, userId, cancellationToken);
    }

    public async Task CreateRemoveFromConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        _cache.Delete(consortiumId.ToGuidAsString());
        await _decorated.CreateRemoveFromConsortiumRequest(consortiumId, organisationId, userId, cancellationToken);
    }
}
