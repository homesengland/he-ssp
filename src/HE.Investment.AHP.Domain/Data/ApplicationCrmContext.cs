using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.Data;

public class ApplicationCrmContext : IApplicationCrmContext
{
    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public ApplicationCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<AhpApplicationDto> GetOrganisationApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = id,
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };
        return await Get(request, cancellationToken);
    }

    public async Task<AhpApplicationDto> GetUserApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = id,
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };

        return await Get(request, cancellationToken);
    }

    public async Task<bool> IsExist(string applicationName, Guid organisationId, CancellationToken cancellationToken)
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

    public async Task<IList<AhpApplicationDto>> GetOrganisationApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<AhpApplicationDto>> GetUserApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, Guid organisationId, IList<string> fieldsToUpdate, CancellationToken cancellationToken)
    {
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_application = CrmResponseSerializer.Serialize(dto),
            invln_fieldstoupdate = FormatFields(fieldsToUpdate),
        };

        return await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            r => r.invln_applicationid,
            cancellationToken);
    }

    public async Task ChangeApplicationStatus(Guid applicationId, Guid organisationId, ApplicationStatus applicationStatus, string? reason, CancellationToken cancellationToken)
    {
        var crmStatus = AhpApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeahpapplicationstatusRequest()
        {
            invln_applicationid = applicationId.ToString(),
            invln_organisationid = organisationId.ToString(),
            invln_userid = _userContext.UserGlobalId,
            invln_newapplicationstatus = crmStatus,
        };

        await _service.ExecuteAsync<invln_changeahpapplicationstatusRequest, invln_changeahpapplicationstatusResponse>(
            request,
            r => r.ResponseName,
            cancellationToken);
    }

    private static string FormatFields(IList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
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

        return response.First();
    }

    private async Task<IList<AhpApplicationDto>> GetAll(invln_getmultipleahpapplicationsRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);
    }
}
