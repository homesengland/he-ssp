using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investment.AHP.Domain.Project.Crm;

internal sealed class RequestCacheProjectCrmContextDecorator : IProjectCrmContext
{
    private readonly IProjectCrmContext _decorated;

    private readonly InMemoryCache<AhpProjectDto, string> _cache = new();

    public RequestCacheProjectCrmContextDecorator(IProjectCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<AhpProjectDto> GetProject(string projectId, string userId, string organisationId, string? consortiumId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            $"{projectId.ToGuidAsString()}_{userId}",
            async () => await _decorated.GetProject(projectId, userId, organisationId, consortiumId, cancellationToken)))!;
    }

    public async Task<PagedResponseDto<AhpProjectDto>> GetProjects(string userId, string organisationId, string? consortiumId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _decorated.GetProjects(userId, organisationId, consortiumId, pagination, cancellationToken);
    }

    public async Task<string> CreateProject(
        string userId,
        string organisationId,
        string? consortiumId,
        string frontDoorProjectId,
        string projectName,
        IList<SiteDto> sites,
        CancellationToken cancellationToken)
    {
        return await _decorated.CreateProject(userId, organisationId, consortiumId, frontDoorProjectId, projectName, sites, cancellationToken);
    }
}
