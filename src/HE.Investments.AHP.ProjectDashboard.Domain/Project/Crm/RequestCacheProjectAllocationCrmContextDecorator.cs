using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;

internal sealed class RequestCacheProjectAllocationCrmContextDecorator : IProjectAllocationCrmContext
{
    private readonly IProjectAllocationCrmContext _decorated;

    private readonly InMemoryCache<ProjectWithAllocationListDto, string> _cache = new();

    public RequestCacheProjectAllocationCrmContextDecorator(IProjectAllocationCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<ProjectWithAllocationListDto> GetProjectAllocations(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            $"{projectId.ToGuidAsString()}_{userId}",
            async () => await _decorated.GetProjectAllocations(projectId, userId, organisationId, consortiumId, cancellationToken)))!;
    }
}
