using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public class HomeTypeCrmContext : IHomeTypeCrmContext
{
    private const string FieldNamesSeparator = ",";

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    private readonly IAccountUserContext _accountUserContext;

    public HomeTypeCrmContext(ICrmService service, IAccountUserContext accountUserContext)
    {
        _service = service;
        _accountUserContext = accountUserContext;
    }

    public async Task<int?> GetHomeTypesStatus(string applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = applicationId,
            invln_appfieldstoretrieve = nameof(invln_scheme.invln_hometypessectioncompletionstatus).ToLowerInvariant(),
        };

        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        return response.FirstOrDefault()?.homeTypesSectionCompletionStatus;
    }

    public async Task SaveHomeTypesStatus(string applicationId, int homeTypesStatus, CancellationToken cancellationToken)
    {
        var application = new AhpApplicationDto { id = applicationId, homeTypesSectionCompletionStatus = homeTypesStatus };
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_application = JsonSerializer.Serialize(application),
            invln_fieldstoupdate = nameof(invln_scheme.invln_hometypessectioncompletionstatus).ToLowerInvariant(),
        };

        await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            x => x.invln_applicationid,
            cancellationToken);
    }

    public async Task<IList<HomeTypeDto>> GetAll(string applicationId, IEnumerable<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_gettypeofhomeslistRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = applicationId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await _service.ExecuteAsync<invln_gettypeofhomeslistRequest, invln_gettypeofhomeslistResponse, IList<HomeTypeDto>>(
            request,
            x => x.invln_hometypeslist,
            cancellationToken);
    }

    public async Task<HomeTypeDto?> GetById(string applicationId, string homeTypeId, IEnumerable<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getsinglehometypeRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await _service.ExecuteAsync<invln_getsinglehometypeRequest, invln_getsinglehometypeResponse, HomeTypeDto?>(
            request,
            x => x.invln_hometype,
            cancellationToken);
    }

    public async Task Remove(string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_deletehometypeRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
        };

        await _service.ExecuteAsync<invln_deletehometypeRequest, invln_deletehometypeResponse>(
            request,
            x => x.ResponseName,
            cancellationToken);
    }

    public async Task<string> Save(HomeTypeDto homeType, IEnumerable<string> fieldsToSave, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_sethometypeRequest
        {
            invln_organisationid = account.AccountId.ToString(),
            invln_userid = account.UserGlobalId.ToString(),
            invln_applicationid = homeType.applicationId,
            invln_hometype = JsonSerializer.Serialize(homeType, _serializerOptions),
            invln_fieldstoset = string.Join(FieldNamesSeparator, fieldsToSave).ToLowerInvariant(),
        };

        return await _service.ExecuteAsync<invln_sethometypeRequest, invln_sethometypeResponse>(
            request,
            x => x.invln_hometypeid,
            cancellationToken);
    }
}
