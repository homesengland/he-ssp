using System.Collections.Concurrent;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Data;

public class RequestCacheApplicationCrmContextDecorator : IApplicationCrmContext
{
    private readonly ConcurrentDictionary<string, AhpApplicationDto> _applicationCache = new();

    private readonly IApplicationCrmContext _decorated;

    public RequestCacheApplicationCrmContextDecorator(IApplicationCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<AhpApplicationDto> GetOrganisationApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        return await GetFromCache(id, organisationId, CrmFields.All.ToList(), _decorated.GetOrganisationApplicationById, cancellationToken);
    }

    public async Task<AhpApplicationDto> GetUserApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        return await GetFromCache(id, organisationId, CrmFields.All.ToList(), _decorated.GetUserApplicationById, cancellationToken);
    }

    public async Task<bool> IsNameExist(string applicationName, Guid organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsNameExist(applicationName, organisationId, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetOrganisationApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        return await _decorated.GetOrganisationApplications(organisationId, fieldsToRetrieve, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetUserApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        return await _decorated.GetUserApplications(organisationId, fieldsToRetrieve, cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, Guid organisationId, IList<string> fieldsToUpdate, CancellationToken cancellationToken)
    {
        RemoveFromCache(dto.id, organisationId);
        return await _decorated.Save(dto, organisationId, fieldsToUpdate, cancellationToken);
    }

    public async Task ChangeApplicationStatus(
        string applicationId,
        Guid organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        CancellationToken cancellationToken)
    {
        RemoveFromCache(applicationId, organisationId);
        await _decorated.ChangeApplicationStatus(applicationId, organisationId, applicationStatus, changeReason, cancellationToken);
    }

    private async Task<AhpApplicationDto> GetFromCache(
        string id,
        Guid organisationId,
        IList<string> fieldsToRetrieve,
        Func<string, Guid, IList<string>, CancellationToken, Task<AhpApplicationDto>> loadAsync,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"{id}-{organisationId}-{string.Join(",", fieldsToRetrieve)}".ToLowerInvariant();
        if (_applicationCache.TryGetValue(cacheKey, out var applicationDto))
        {
            return applicationDto;
        }

        applicationDto = await loadAsync(id, organisationId, fieldsToRetrieve, cancellationToken);
        _applicationCache.TryAdd(cacheKey, applicationDto);

        return applicationDto;
    }

    private void RemoveFromCache(string applicationId, Guid organisationId)
    {
        var keys = _applicationCache
            .Where(x => x.Key.StartsWith($"{applicationId}-{organisationId}-".ToLowerInvariant(), StringComparison.InvariantCulture))
            .Select(x => x.Key);

        foreach (var key in keys)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _applicationCache.Remove(key, out _);
            }
        }
    }
}
