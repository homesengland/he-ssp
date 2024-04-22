using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.Application.Crm;

public class ApplicationCrmContext : IApplicationCrmContext
{
    private static readonly string ApplicationListCrmFields =
        string.Join(
                ",",
                nameof(invln_scheme.invln_schemename),
                nameof(invln_scheme.invln_Tenure),
                nameof(invln_ExternalStatus),
                nameof(invln_scheme.invln_applicationid),
                nameof(invln_scheme.invln_Site),
                nameof(invln_scheme.invln_fundingrequired),
                nameof(invln_scheme.invln_noofhomes),
                nameof(invln_scheme.invln_pplicationid),
                nameof(invln_scheme.invln_lastexternalmodificationby),
                nameof(invln_scheme.invln_lastexternalmodificationon))
            .ToLowerInvariant();

    private static readonly string ApplicationCrmFields =
        string.Join(
                ",",
                nameof(invln_scheme.invln_schemename),
                nameof(invln_scheme.invln_Tenure),
                nameof(invln_ExternalStatus),
                nameof(invln_scheme.invln_applicationid),
                nameof(invln_scheme.invln_Site),
                nameof(invln_scheme.invln_schemeinformationsectioncompletionstatus),
                nameof(invln_scheme.invln_hometypessectioncompletionstatus),
                nameof(invln_scheme.invln_financialdetailssectioncompletionstatus),
                nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus),
                nameof(invln_scheme.invln_lastexternalmodificationby),
                nameof(invln_scheme.invln_lastexternalmodificationon),
                nameof(invln_scheme.invln_pplicationid),
                nameof(invln_scheme.invln_DateSubmitted),
                nameof(invln_scheme.invln_submitedby),
                nameof(invln_scheme.invln_PreviousExternalStatus),
                nameof(invln_scheme.invln_fundingrequired),
                nameof(invln_scheme.invln_noofhomes),
                nameof(invln_scheme.invln_currentlandvalue),
                nameof(invln_scheme.invln_expectedoncosts),
                nameof(invln_scheme.invln_expectedonworks),
                nameof(invln_scheme.invln_affordabilityevidence),
                nameof(invln_scheme.invln_sharedownershipsalesrisk),
                nameof(invln_scheme.invln_meetinglocalpriorities),
                nameof(invln_scheme.invln_meetinglocalhousingneed),
                nameof(invln_scheme.invln_discussionswithlocalstakeholders),
                nameof(invln_scheme.invln_actualacquisitioncost),
                nameof(invln_scheme.invln_expectedacquisitioncost),
                nameof(invln_scheme.invln_publicland),
                nameof(invln_scheme.invln_borrowingagainstrentalincome),
                nameof(invln_scheme.invln_fundingfromopenmarkethomesonthisscheme),
                nameof(invln_scheme.invln_fundingfromopenmarkethomesnotonthisscheme),
                nameof(invln_scheme.invln_ownresources),
                nameof(invln_scheme.invln_recycledcapitalgrantfund),
                nameof(invln_scheme.invln_totalinitialsalesincome),
                nameof(invln_scheme.invln_othercapitalsources),
                nameof(invln_scheme.invln_transfervalue),
                nameof(invln_scheme.invln_grantsfromcountycouncil),
                nameof(invln_scheme.invln_grantsfromdhscextracare),
                nameof(invln_scheme.invln_grantsfromlocalauthority1),
                nameof(invln_scheme.invln_grantsfromsocialservices),
                nameof(invln_scheme.invln_grantsfromdhscnhsorotherhealth),
                nameof(invln_scheme.invln_grantsfromthelottery),
                nameof(invln_scheme.invln_grantsfromotherpublicbodies),
                nameof(invln_scheme.invln_programmelookup))
            .ToLowerInvariant();

    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public ApplicationCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<AhpApplicationDto> GetOrganisationApplicationById(string id, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = id,
            invln_appfieldstoretrieve = ApplicationCrmFields,
        };
        return await Get(request, cancellationToken);
    }

    public async Task<AhpApplicationDto> GetUserApplicationById(string id, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = id,
            invln_appfieldstoretrieve = ApplicationCrmFields,
        };

        return await Get(request, cancellationToken);
    }

    public async Task<bool> IsNameExist(string applicationName, Guid organisationId, CancellationToken cancellationToken)
    {
        var dto = new AhpApplicationDto
        {
            name = applicationName,
        };

        var request = new invln_checkifapplicationwithgivennameexistsRequest
        {
            invln_application = CrmResponseSerializer.Serialize(dto),
            invln_organisationid = organisationId.ToString(),
        };

        var response = await _service.ExecuteAsync<invln_checkifapplicationwithgivennameexistsRequest, invln_checkifapplicationwithgivennameexistsResponse>(
            request,
            r => r.invln_applicationexists,
            cancellationToken);

        return bool.TryParse(response, out var result) && result;
    }

    public async Task<IList<AhpApplicationDto>> GetOrganisationApplications(Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = ApplicationListCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetUserApplications(Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = ApplicationListCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_application = CrmResponseSerializer.Serialize(dto),
            invln_fieldstoupdate = ApplicationCrmFields,
        };

        return await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            r => r.invln_applicationid,
            cancellationToken);
    }

    public async Task ChangeApplicationStatus(
        string applicationId,
        Guid organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        CancellationToken cancellationToken)
    {
        var crmStatus = AhpApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeahpapplicationstatusRequest
        {
            invln_applicationid = applicationId,
            invln_organisationid = organisationId.ToString(),
            invln_userid = _userContext.UserGlobalId,
            invln_newapplicationstatus = crmStatus,
            invln_changereason = changeReason ?? string.Empty,
        };

        await _service.ExecuteAsync<invln_changeahpapplicationstatusRequest, invln_changeahpapplicationstatusResponse>(
            request,
            r => r.ResponseName,
            cancellationToken);
    }

    private async Task<AhpApplicationDto> Get(invln_getahpapplicationRequest request, CancellationToken cancellationToken)
    {
        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("AhpApplication", request.invln_applicationid);
        }

        return response[0];
    }

    private async Task<IList<AhpApplicationDto>> GetAll(invln_getmultipleahpapplicationsRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);
    }
}
