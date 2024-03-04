using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Data;

public class SiteCrmContext : ISiteCrmContext
{
    private readonly ICrmService _service;

    public SiteCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<PagedResponseDto<SiteDto>> Get(PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplesitesRequest, invln_getmultiplesitesResponse, PagedResponseDto<SiteDto>>(
            new invln_getmultiplesitesRequest
            {
                invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
                invln_fieldstoretrieve = FormatFields(SiteCrmFields.Fields),
            },
            r => r.invln_sites,
            cancellationToken);
    }

    public async Task<SiteDto?> GetById(string id, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getsinglesiteRequest, invln_getsinglesiteResponse, SiteDto>(
            new invln_getsinglesiteRequest
            {
                invln_siteid = id,
                invln_fieldstoretrieve = FormatFields(SiteCrmFields.Fields),
            },
            r => r.invln_site,
            cancellationToken);
    }

    public async Task<bool> Exist(string name, CancellationToken cancellationToken)
    {
        var response = await _service.ExecuteAsync<invln_checkifsitewithgivennameexistsRequest, invln_checkifsitewithgivennameexistsResponse>(
            new invln_checkifsitewithgivennameexistsRequest
            {
                invln_sitename = name,
            },
            r => r.invln_siteexists,
            cancellationToken);

        return bool.TryParse(response, out var result) && result;
    }

    public async Task<string> Save(SiteDto dto, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_setsiteRequest, invln_setsiteResponse>(
            new invln_setsiteRequest
            {
                invln_siteid = dto.id,
                invln_fieldstoset = FormatFields(SiteCrmFields.Fields),
                invln_site = CrmResponseSerializer.Serialize(dto),
            },
            r => r.invln_siteid,
            cancellationToken);
    }

    private static string FormatFields(IReadOnlyList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
