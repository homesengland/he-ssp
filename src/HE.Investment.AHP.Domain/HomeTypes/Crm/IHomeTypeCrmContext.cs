using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public interface IHomeTypeCrmContext
{
    Task<IList<HomeTypeDto>> GetAllOrganisationHomeTypes(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken);

    Task<IList<HomeTypeDto>> GetAllUserHomeTypes(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken);

    Task<HomeTypeDto?> GetOrganisationHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        CancellationToken cancellationToken);

    Task<HomeTypeDto?> GetUserHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        CancellationToken cancellationToken);

    Task Remove(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken);

    Task<string> Save(HomeTypeDto homeType, Guid organisationId, CancellationToken cancellationToken);
}
