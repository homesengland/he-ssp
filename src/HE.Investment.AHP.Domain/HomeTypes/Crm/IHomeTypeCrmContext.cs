using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public interface IHomeTypeCrmContext
{
    Task<int?> GetHomeTypesStatus(string applicationId, CancellationToken cancellationToken);

    Task SaveHomeTypesStatus(string applicationId, int homeTypesStatus, CancellationToken cancellationToken);

    Task<IList<HomeTypeDto>> GetAll(string applicationId, IEnumerable<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<HomeTypeDto?> GetById(string applicationId, string homeTypeId, IEnumerable<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task Remove(string applicationId, string homeTypeId, CancellationToken cancellationToken);

    Task<string> Save(HomeTypeDto homeType, IEnumerable<string> fieldsToSave, CancellationToken cancellationToken);
}
