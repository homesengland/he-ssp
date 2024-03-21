using System.Collections.Concurrent;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public class CacheProjectCrmContext : IProjectCrmContext
{
    private readonly ConcurrentDictionary<string, FrontDoorProjectDto> _cache = new();

    private readonly IProjectCrmContext _context;

    public CacheProjectCrmContext(IProjectCrmContext context)
    {
        _context = context;
    }

    public Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return _context.GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return _context.GetUserProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await GetFromCache(projectId, userGlobalId, organisationId, _context.GetOrganisationProjectById, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await GetFromCache(projectId, userGlobalId, organisationId, _context.GetUserProjectById, cancellationToken);
    }

    public Task<bool> IsThereProjectWithName(string projectName, Guid organisationId, CancellationToken cancellationToken)
    {
        return _context.IsThereProjectWithName(projectName, organisationId, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        ReplaceCache(dto.ProjectId, dto);
        return await _context.Save(dto, userAccount, cancellationToken);
    }

    private async Task<FrontDoorProjectDto> GetFromCache(string projectId, string userGlobalId, Guid organisationId, Func<string, string, Guid, CancellationToken, Task<FrontDoorProjectDto>> getFromContext, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(projectId, out var project))
        {
            return project;
        }

        var frontDoorProject = await getFromContext(projectId, userGlobalId, organisationId, cancellationToken);
        _cache.TryAdd(projectId, frontDoorProject);

        return frontDoorProject;
    }

    private void ReplaceCache(string projectId, FrontDoorProjectDto project)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            return;
        }

        _cache.AddOrUpdate(projectId, project, (_, _) => project);
    }
}
