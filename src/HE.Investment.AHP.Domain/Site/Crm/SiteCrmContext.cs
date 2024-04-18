using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Site.Crm;

public class SiteCrmContext : ISiteCrmContext
{
    private static readonly string SiteCrmFields =
        string.Join(
                ",",
                nameof(invln_Sites.invln_SitesId),
                nameof(invln_Sites.invln_AccountId),
                nameof(invln_Sites.invln_CreatedByContactId),
                nameof(invln_Sites.invln_sitename),
                nameof(invln_Sites.invln_externalsitestatus),
                nameof(invln_Sites.invln_s106agreementinplace),
                nameof(invln_Sites.invln_developercontributionsforah),
                nameof(invln_Sites.invln_siteis100affordable),
                nameof(invln_Sites.invln_homesintheapplicationareadditional),
                nameof(invln_Sites.invln_anyrestrictionsinthes106),
                nameof(invln_Sites.invln_localauthorityconfirmationofadditionality),
                nameof(invln_Sites.invln_LocalAuthority),
                nameof(invln_Sites.invln_planningstatus),
                nameof(invln_Sites.invln_planningreferencenumber),
                nameof(invln_Sites.invln_detailedplanningapprovaldate),
                nameof(invln_Sites.invln_furtherstepsrequired),
                nameof(invln_Sites.invln_applicationfordetailedplanningsubmitted),
                nameof(invln_Sites.invln_expectedplanningapproval),
                nameof(invln_Sites.invln_outlineplanningapprovaldate),
                nameof(invln_Sites.invln_planningsubmissiondate),
                nameof(invln_Sites.invln_grantfundingforallhomes),
                nameof(invln_Sites.invln_landregistrytitle),
                nameof(invln_Sites.invln_landregistrytitlenumber),
                nameof(invln_Sites.invln_invlngrantfundingforallhomescoveredbytit),
                nameof(invln_Sites.invln_nationaldesignguideelements),
                nameof(invln_Sites.invln_assessedforbhl),
                nameof(invln_Sites.invln_bhlgreentrafficlights),
                nameof(invln_Sites.invln_landstatus),
                nameof(invln_Sites.invln_workstenderingstatus),
                nameof(invln_Sites.invln_maincontractorname),
                nameof(invln_Sites.invln_sme),
                nameof(invln_Sites.invln_intentiontoworkwithsme),
                nameof(invln_Sites.invln_strategicsite),
                nameof(invln_Sites.invln_StrategicSiteN),
                nameof(invln_Sites.invln_TypeofSite),
                nameof(invln_Sites.invln_greenbelt),
                nameof(invln_Sites.invln_regensite),
                nameof(invln_Sites.invln_streetfrontinfill),
                nameof(invln_Sites.invln_travellerpitchsite),
                nameof(invln_Sites.invln_travellerpitchsitetype),
                nameof(invln_Sites.invln_Ruralclassification),
                nameof(invln_Sites.invln_RuralExceptionSite),
                nameof(invln_Sites.invln_ActionstoReduce),
                nameof(invln_Sites.invln_mmcuse),
                nameof(invln_Sites.invln_mmcplans),
                nameof(invln_Sites.invln_mmcexpectedimpact),
                nameof(invln_Sites.invln_mmcbarriers),
                nameof(invln_Sites.invln_mmcimpact),
                nameof(invln_Sites.invln_mmccategories),
                nameof(invln_Sites.invln_mmccategory1subcategories),
                nameof(invln_Sites.invln_mmccategory2subcategories),
                nameof(invln_Sites.invln_procurementmechanisms))
            .ToLowerInvariant();

    private readonly ICrmService _service;

    public SiteCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<PagedResponseDto<SiteDto>> GetOrganisationSites(Guid organisationId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplesitesRequest, invln_getmultiplesitesResponse, PagedResponseDto<SiteDto>>(
            new invln_getmultiplesitesRequest
            {
                invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
                invln_fieldstoretrieve = SiteCrmFields,
                invln_accountid = organisationId.ToString(),
            },
            r => r.invln_sites,
            cancellationToken);
    }

    public async Task<PagedResponseDto<SiteDto>> GetUserSites(string userGlobalId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultiplesitesRequest, invln_getmultiplesitesResponse, PagedResponseDto<SiteDto>>(
            new invln_getmultiplesitesRequest
            {
                invln_pagingrequest = CrmResponseSerializer.Serialize(pagination),
                invln_fieldstoretrieve = SiteCrmFields,
                invln_externalcontactid = userGlobalId,
            },
            r => r.invln_sites,
            cancellationToken);
    }

    public async Task<SiteDto?> GetById(string siteId, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getsinglesiteRequest, invln_getsinglesiteResponse, SiteDto>(
            new invln_getsinglesiteRequest
            {
                invln_siteid = siteId,
                invln_fieldstoretrieve = SiteCrmFields,
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

    public async Task<string> Save(Guid organisationId, string userGlobalId, SiteDto dto, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_setsiteRequest, invln_setsiteResponse>(
            new invln_setsiteRequest
            {
                invln_siteid = dto.id,
                invln_fieldstoset = SiteCrmFields,
                invln_site = CrmResponseSerializer.Serialize(dto),
                invln_accountid = organisationId.ToString(),
                invln_externalcontactid = userGlobalId,
            },
            r => r.invln_siteid,
            cancellationToken);
    }
}
