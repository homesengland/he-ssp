using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public class HomeTypeCrmContext : IHomeTypeCrmContext
{
    private const string FieldNamesSeparator = ",";

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public HomeTypeCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<int?> GetHomeTypesStatus(string applicationId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_appfieldstoretrieve = nameof(invln_scheme.invln_hometypessectioncompletionstatus).ToLowerInvariant(),
        };

        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        return response.FirstOrDefault()?.homeTypesSectionCompletionStatus;
    }

    public async Task SaveHomeTypesStatus(string applicationId, Guid organisationId, int homeTypesStatus, CancellationToken cancellationToken)
    {
        var application = new AhpApplicationDto { id = applicationId, homeTypesSectionCompletionStatus = homeTypesStatus };
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_application = JsonSerializer.Serialize(application),
            invln_fieldstoupdate = nameof(invln_scheme.invln_hometypessectioncompletionstatus).ToLowerInvariant(),
        };

        await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            x => x.invln_applicationid,
            cancellationToken);
    }

    public async Task<IList<HomeTypeDto>> GetAllOrganisationHomeTypes(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_gettypeofhomeslistRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<HomeTypeDto>> GetAllUserHomeTypes(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_gettypeofhomeslistRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<HomeTypeDto?> GetOrganisationHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsinglehometypeRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task<HomeTypeDto?> GetUserHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsinglehometypeRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task Remove(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_deletehometypeRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
        };

        await _service.ExecuteAsync<invln_deletehometypeRequest, invln_deletehometypeResponse>(
            request,
            x => x.ResponseName,
            cancellationToken);
    }

    public async Task<string> Save(HomeTypeDto homeType, Guid organisationId, IEnumerable<string> fieldsToSave, CancellationToken cancellationToken)
    {
        var request = new invln_sethometypeRequest
        {
            invln_organisationid = organisationId.ToString(),
            invln_userid = _userContext.UserGlobalId,
            invln_applicationid = homeType.applicationId,
            invln_hometype = JsonSerializer.Serialize(homeType, _serializerOptions),
            invln_fieldstoset = string.Join(FieldNamesSeparator, fieldsToSave).ToLowerInvariant(),
        };

        return await _service.ExecuteAsync<invln_sethometypeRequest, invln_sethometypeResponse>(
            request,
            x => x.invln_hometypeid,
            cancellationToken);
    }

    private async Task<HomeTypeDto?> GetSingle(invln_getsinglehometypeRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getsinglehometypeRequest, invln_getsinglehometypeResponse, HomeTypeDto?>(
            request,
            x => x.invln_hometype,
            cancellationToken);
    }

    private async Task<IList<HomeTypeDto>> GetAll(invln_gettypeofhomeslistRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_gettypeofhomeslistRequest, invln_gettypeofhomeslistResponse, IList<HomeTypeDto>>(
            request,
            x => x.invln_hometypeslist,
            cancellationToken);
    }
}
