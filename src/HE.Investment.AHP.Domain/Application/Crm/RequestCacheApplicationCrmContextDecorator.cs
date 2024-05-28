using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investment.AHP.Domain.Application.Crm;

public class RequestCacheApplicationCrmContextDecorator : IApplicationCrmContext
{
    private readonly InMemoryCache<AhpApplicationDto, string> _cache = new();

    private readonly IApplicationCrmContext _decorated;

    public RequestCacheApplicationCrmContextDecorator(IApplicationCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<AhpApplicationDto> GetOrganisationApplicationById(string id, string organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            id.ToGuidAsString(),
            async () => await _decorated.GetOrganisationApplicationById(id, organisationId, cancellationToken)))!;
    }

    public async Task<AhpApplicationDto> GetUserApplicationById(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            id.ToGuidAsString(),
            async () => await _decorated.GetUserApplicationById(id, organisationId, userId, cancellationToken)))!;
    }

    public async Task<bool> IsNameExist(string applicationName, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsNameExist(applicationName, organisationId, cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var applicationId = await _decorated.Save(dto, organisationId, userId, cancellationToken);
        _cache.Delete(applicationId);

        return applicationId;
    }

    public async Task ChangeApplicationStatus(
        string applicationId,
        string organisationId,
        string userId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        bool representationsAndWarranties,
        CancellationToken cancellationToken)
    {
        _cache.Delete(applicationId.ToGuidAsString());
        await _decorated.ChangeApplicationStatus(applicationId, organisationId, userId, applicationStatus, changeReason, representationsAndWarranties, cancellationToken);
    }
}
