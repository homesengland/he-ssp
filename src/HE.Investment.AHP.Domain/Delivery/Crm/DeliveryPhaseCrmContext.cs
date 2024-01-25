using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public class DeliveryPhaseCrmContext : IDeliveryPhaseCrmContext
{
    private const string FieldNamesSeparator = ",";

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public DeliveryPhaseCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<int?> GetDeliveryStatus(string applicationId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_appfieldstoretrieve = nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus).ToLowerInvariant(),
        };

        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        return response.FirstOrDefault()?.deliveryPhasesSectionCompletionStatus;
    }

    public async Task SaveDeliveryStatus(string applicationId, Guid organisationId, int deliveryStatus, CancellationToken cancellationToken)
    {
        var application = new AhpApplicationDto { id = applicationId, deliveryPhasesSectionCompletionStatus = deliveryStatus };
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_application = JsonSerializer.Serialize(application),
            invln_fieldstoupdate = nameof(invln_scheme.invln_deliveryphasessectioncompletionstatus).ToLowerInvariant(),
        };

        await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            x => x.invln_applicationid,
            cancellationToken);
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = string.Empty,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = _userContext.UserGlobalId,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = string.Empty,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_deliveryPhaseId = deliveryPhaseId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_deliveryPhaseId = deliveryPhaseId,
            invln_fieldstoretrieve = string.Join(FieldNamesSeparator, fieldsToRetrieve).ToLowerInvariant(),
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task Remove(string applicationId, string deliveryPhaseId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_deletedeliveryphaseRequest
        {
            invln_userId = _userContext.UserGlobalId,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_deliveryPhaseId = deliveryPhaseId,
        };

        await _service.ExecuteAsync<invln_deletedeliveryphaseRequest, invln_deletedeliveryphaseResponse>(
            request,
            x => x.ResponseName,
            cancellationToken);
    }

    public async Task<string> Save(DeliveryPhaseDto deliveryPhase, Guid organisationId, IEnumerable<string> fieldsToSave, CancellationToken cancellationToken)
    {
        var request = new invln_setdeliveryphaseRequest
        {
            invln_organisationId = organisationId.ToString(),
            invln_userId = _userContext.UserGlobalId,
            invln_applicationId = deliveryPhase.applicationId,
            invln_deliveryPhase = JsonSerializer.Serialize(deliveryPhase, _serializerOptions),
            invln_fieldstoset = string.Join(FieldNamesSeparator, fieldsToSave).ToLowerInvariant(),
        };

        return await _service.ExecuteAsync<invln_setdeliveryphaseRequest, invln_setdeliveryphaseResponse>(
            request,
            x => x.invln_deliveryphaseid,
            cancellationToken);
    }

    private async Task<DeliveryPhaseDto?> GetSingle(invln_getsingledeliveryphaseRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getsingledeliveryphaseRequest, invln_getsingledeliveryphaseResponse, DeliveryPhaseDto?>(
            request,
            x => x.invln_deliveryPhase,
            cancellationToken);
    }

    private async Task<IList<DeliveryPhaseDto>> GetAll(invln_getmultipledeliveryphaseRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getmultipledeliveryphaseRequest, invln_getmultipledeliveryphaseResponse, IList<DeliveryPhaseDto>>(
            request,
            x => x.invln_deliveryPhaseList,
            cancellationToken);
    }
}
