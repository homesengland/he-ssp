using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Crm;

internal sealed class RequestCacheProjectContextDecorator : IProjectContext
{
    private readonly InMemoryCache<FrontDoorProjectDto, string> _cache = new();

    private readonly IProjectContext _decorated;

    public RequestCacheProjectContextDecorator(IProjectContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetUserProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            projectId.ToGuidAsString(),
            async () => await _decorated.GetOrganisationProjectById(projectId, userGlobalId, organisationId, cancellationToken)))!;
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            projectId.ToGuidAsString(),
            async () => await _decorated.GetUserProjectById(projectId, userGlobalId, organisationId, cancellationToken)))!;
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsThereProjectWithName(projectName, organisationId, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        dto.ProjectId = await _decorated.Save(dto, userGlobalId, organisationId, cancellationToken);
        _cache.ReplaceCache(dto.ProjectId, dto);

        return dto.ProjectId;
    }
}
