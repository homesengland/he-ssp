using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public class DeliveryPhaseCrmContext : IDeliveryPhaseCrmContext
{
    private static readonly string DeliveryPhaseCrmFields =
        string.Join(
                ",",
                nameof(invln_DeliveryPhase.invln_phasename),
                nameof(invln_DeliveryPhase.CreatedOn),
                nameof(invln_DeliveryPhase.invln_iscompleted),
                nameof(invln_DeliveryPhase.invln_buildactivitytype),
                nameof(invln_DeliveryPhase.invln_rehabactivitytype),
                nameof(invln_DeliveryPhase.invln_reconfiguringexistingproperties),
                nameof(invln_DeliveryPhase.invln_acquisitiondate),
                nameof(invln_DeliveryPhase.invln_acquisitionmilestoneclaimdate),
                nameof(invln_DeliveryPhase.invln_startonsitedate),
                nameof(invln_DeliveryPhase.invln_startonsitemilestoneclaimdate),
                nameof(invln_DeliveryPhase.invln_completiondate),
                nameof(invln_DeliveryPhase.invln_completionmilestoneclaimdate),
                nameof(invln_DeliveryPhase.invln_urbrequestingearlymilestonepayments),
                nameof(invln_DeliveryPhase.invln_nbrh),
                nameof(invln_DeliveryPhase.invln_AcquisitionPercentageValue),
                nameof(invln_DeliveryPhase.invln_StartOnSitePercentageValue),
                nameof(invln_DeliveryPhase.invln_CompletionPercentageValue),
                nameof(invln_DeliveryPhase.invln_ClaimingtheMilestoneConfirmed),
                nameof(invln_DeliveryPhase.invln_AllowAmendmentstoMilestoneProportions),
                nameof(invln_DeliveryPhase.invln_AcquisitionValue),
                nameof(invln_DeliveryPhase.invln_CompletionValue),
                nameof(invln_DeliveryPhase.invln_StartOnSiteValue),
                nameof(invln_DeliveryPhase.invln_sumofcalculatedfounds))
            .ToLowerInvariant();

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public DeliveryPhaseCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = string.Empty,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = _userContext.UserGlobalId,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = string.Empty,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_deliveryPhaseId = deliveryPhaseId,
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationId = organisationId.ToString(),
            invln_applicationId = applicationId,
            invln_deliveryPhaseId = deliveryPhaseId,
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
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

    public async Task<string> Save(DeliveryPhaseDto deliveryPhase, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_setdeliveryphaseRequest
        {
            invln_organisationId = organisationId.ToString(),
            invln_userId = _userContext.UserGlobalId,
            invln_applicationId = deliveryPhase.applicationId,
            invln_deliveryPhase = JsonSerializer.Serialize(deliveryPhase, _serializerOptions),
            invln_fieldstoset = DeliveryPhaseCrmFields,
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
