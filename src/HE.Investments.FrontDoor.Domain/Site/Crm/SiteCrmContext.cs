using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.FrontDoor.Shared.Project.Crm;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

public class SiteCrmContext : ISiteCrmContext
{
    private readonly ICrmService _service;

    private readonly IFeatureManager _featureManager;

    public SiteCrmContext(ICrmService service, IFeatureManager featureManager)
    {
        _service = service;
        _featureManager = featureManager;
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(
        string projectId,
        UserAccount userAccount,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectssiteRequest
        {
            invln_frontdoorprojectid = projectId,
            invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
            invln_fieldstoretrieve = ProjectSiteCrmFields.SiteToRead.FormatFields(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        return await _service
            .ExecuteAsync<
                invln_getmultiplefrontdoorprojectssiteRequest,
                invln_getmultiplefrontdoorprojectssiteResponse,
                PagedResponseDto<FrontDoorProjectSiteDto>>(
                request,
                r => r.invln_frontdoorprojectsites,
                cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getsinglefrontdoorprojectsiteRequest
        {
            invln_frontdoorprojectsiteid = siteId,
            invln_frontdoorprojectid = projectId,
            invln_fieldstoretrieve = ProjectSiteCrmFields.SiteToRead.FormatFields(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        return await _service.ExecuteAsync<invln_getsinglefrontdoorprojectsiteRequest, invln_getsinglefrontdoorprojectsiteResponse, FrontDoorProjectSiteDto>(
            request,
            r => r.invln_frontdoorprojectsite,
            cancellationToken);
    }

    public async Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_setfrontdoorsiteRequest
        {
            invln_frontdoorprojectid = projectId,
            invln_frontdoorsiteid = dto.SiteId,
            invln_entityfieldsparameters = CrmResponseSerializer.Serialize(dto),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        return await _service.ExecuteAsync<invln_setfrontdoorsiteRequest, invln_setfrontdoorsiteResponse>(
            request,
            r => r.invln_frontdoorsiteid,
            cancellationToken);
    }

    public async Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_deactivatefrontdoorsiteRequest
        {
            invln_frontdoorsiteid = siteId,
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        return await _service.ExecuteAsync<invln_deactivatefrontdoorsiteRequest, invln_deactivatefrontdoorsiteResponse>(
            request,
            r => r.invln_sitedeactivated,
            cancellationToken);
    }
}
