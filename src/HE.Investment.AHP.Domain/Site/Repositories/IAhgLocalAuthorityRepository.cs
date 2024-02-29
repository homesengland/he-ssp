using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface IAhgLocalAuthorityRepository
{
    Task<PagedResponseDto<AhgLocalAuthorityDto>> Get(PagingRequestDto pagingRequest, string searchPhrase, CancellationToken cancellationToken);
}
