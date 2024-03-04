using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public class ProjectCrmContext : IProjectCrmContext
{
    private static readonly IList<string> ToUpdate = new List<string>
    {
        nameof(invln_scheme.invln_schemename),
        nameof(invln_scheme.invln_applicationid),
    };

    private readonly ICrmService _service;

    public ProjectCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<IList<ProjectDto>> GetOrganisationProjects(Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = FormatFields(ToUpdate),
        };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<IList<ProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = userGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_appfieldstoretrieve = FormatFields(ToUpdate),
        };

        return await GetProjects(request, cancellationToken);
    }

    public async Task<ProjectDto> GetOrganisationProjectById(string projectId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = projectId,
            invln_appfieldstoretrieve = FormatFields(ToUpdate),
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<ProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = userGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = projectId,
            invln_appfieldstoretrieve = FormatFields(ToUpdate),
        };

        return await GetProject(request, cancellationToken);
    }

    public async Task<string> Save(ProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = userAccount.UserGlobalId.ToString(),
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            invln_application = CrmResponseSerializer.Serialize(dto),
        };

        return await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            r => r.invln_applicationid,
            cancellationToken);
    }

    private static string FormatFields(IList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }

    private async Task<IList<ProjectDto>> GetProjects(invln_getmultipleahpapplicationsRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<ProjectDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);
    }

    private async Task<ProjectDto> GetProject(
        invln_getahpapplicationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<ProjectDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("Project", request.invln_applicationid);
        }

        return response[0];
    }
}
