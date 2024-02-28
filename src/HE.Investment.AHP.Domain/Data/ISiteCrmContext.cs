using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Data;

public interface ISiteCrmContext
{
    Task<IList<SiteDto>> GetAll(CancellationToken cancellationToken);

    Task<SiteDto?> GetById(string id, CancellationToken cancellationToken);

    Task<bool> Exist(string name, CancellationToken cancellationToken);

    Task<string> Save(SiteDto dto, CancellationToken cancellationToken);
}
