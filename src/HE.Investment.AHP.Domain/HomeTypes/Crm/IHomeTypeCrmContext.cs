using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public interface IHomeTypeCrmContext
{
    Task<IList<HomeTypeDto>> GetAllOrganisationHomeTypes(
        string applicationId,
        string organisationId,
        CancellationToken cancellationToken);

    Task<IList<HomeTypeDto>> GetAllUserHomeTypes(
        string applicationId,
        string organisationId,
        CancellationToken cancellationToken);

    Task<HomeTypeDto?> GetOrganisationHomeTypeById(
        string applicationId,
        string homeTypeId,
        string organisationId,
        CancellationToken cancellationToken);

    Task<HomeTypeDto?> GetUserHomeTypeById(
        string applicationId,
        string homeTypeId,
        string organisationId,
        CancellationToken cancellationToken);

    Task Remove(string applicationId, string homeTypeId, string organisationId, CancellationToken cancellationToken);

    Task<string> Save(HomeTypeDto homeType, string organisationId, CancellationToken cancellationToken);
}
