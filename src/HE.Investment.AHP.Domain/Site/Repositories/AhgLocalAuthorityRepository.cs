using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Data;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class AhgLocalAuthorityRepository : IAhgLocalAuthorityRepository
{
    private readonly ICrmService _service;

    public AhgLocalAuthorityRepository(ICrmService service)
    {
        _service = service;
    }

    public async Task<PagedResponseDto<AhgLocalAuthorityDto>> Get(PagingRequestDto pagingRequest, string searchPhrase, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplelocalauthoritiesRequest, invln_getmultiplelocalauthoritiesResponse, PagedResponseDto<AhgLocalAuthorityDto>>(
            new invln_getmultiplelocalauthoritiesRequest
            {
                invln_searchphrase = searchPhrase,
                invln_pagingrequest = CrmResponseSerializer.Serialize(pagingRequest),
                invln_fieldstoretrieve = FormatFields(LocalAuthorityCrmFields.Fields),
            },
            r => r.invln_localauthorities,
            cancellationToken);
    }

    private static string FormatFields(IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
