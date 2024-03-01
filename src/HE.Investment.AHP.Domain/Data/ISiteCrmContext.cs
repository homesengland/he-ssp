using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Data;

public interface ISiteCrmContext
{
    Task<PagedResponseDto<SiteDto>> Get(PagingRequestDto pagination, CancellationToken cancellationToken);

    Task<SiteDto?> GetById(string id, CancellationToken cancellationToken);

    Task<bool> Exist(string name, CancellationToken cancellationToken);

    Task<string> Save(SiteDto dto, CancellationToken cancellationToken);
}
