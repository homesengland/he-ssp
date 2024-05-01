using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
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
            ShortGuid.ToGuidAsString(id),
            async () => await _decorated.GetOrganisationApplicationById(id, organisationId, cancellationToken)))!;
    }

    public async Task<AhpApplicationDto> GetUserApplicationById(string id, string organisationId, CancellationToken cancellationToken)
    {
        return (await _cache.GetFromCache(
            ShortGuid.ToGuidAsString(id),
            async () => await _decorated.GetUserApplicationById(id, organisationId, cancellationToken)))!;
    }

    public async Task<bool> IsNameExist(string applicationName, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.IsNameExist(applicationName, organisationId, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetOrganisationApplications(string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetOrganisationApplications(organisationId, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetUserApplications(string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetUserApplications(organisationId, cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, string organisationId, CancellationToken cancellationToken)
    {
        var applicationId = await _decorated.Save(dto, organisationId, cancellationToken);
        _cache.Delete(applicationId);

        return applicationId;
    }

    public async Task ChangeApplicationStatus(
        string applicationId,
        string organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        bool representationsAndWarranties,
        CancellationToken cancellationToken)
    {
        _cache.Delete(ShortGuid.ToGuidAsString(applicationId));
        await _decorated.ChangeApplicationStatus(applicationId, organisationId, applicationStatus, changeReason, representationsAndWarranties, cancellationToken);
    }
}
