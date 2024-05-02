using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

internal sealed class RequestCacheProjectCrmContextDecorator : IProjectCrmContext
{
    private readonly InMemoryCache<FrontDoorProjectDto, string> _cache = new();

    private readonly IProjectCrmContext _decorated;

    public RequestCacheProjectCrmContextDecorator(IProjectCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetUserProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            projectId,
            async () => await _decorated.GetOrganisationProjectById(projectId, userGlobalId, organisationId, cancellationToken)))!;
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            projectId,
            async () => await _decorated.GetUserProjectById(projectId, userGlobalId, organisationId, cancellationToken)))!;
    }

    public async Task<bool> IsThereProjectWithName(string projectName, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsThereProjectWithName(projectName, organisationId, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        dto.ProjectId = await _decorated.Save(dto, userAccount, cancellationToken);
        _cache.ReplaceCache(dto.ProjectId, dto);

        return dto.ProjectId;
    }
}
