using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Data;

public interface IApplicationCrmContext
{
    Task<AhpApplicationDto> GetById(string id, IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<bool> IsExist(string applicationName, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetAll(IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<string> Save(AhpApplicationDto dto, IList<string> fieldsToUpdate, CancellationToken cancellationToken);
}
