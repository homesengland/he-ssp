using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

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
                nameof(invln_DeliveryPhase.invln_sumofcalculatedfounds),
                nameof(invln_DeliveryPhase.StatusCode))
            .ToLowerInvariant();

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    public DeliveryPhaseCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(
        string applicationId,
        string organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = string.Empty,
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_applicationId = applicationId.ToGuidAsString(),
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(
        string applicationId,
        string organisationId,
        string userId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getmultipledeliveryphaseRequest
        {
            invln_userId = userId,
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_applicationId = applicationId.ToGuidAsString(),
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        string organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = string.Empty,
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_applicationId = applicationId.ToGuidAsString(),
            invln_deliveryPhaseId = deliveryPhaseId.ToGuidAsString(),
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        string organisationId,
        string userId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsingledeliveryphaseRequest
        {
            invln_userid = userId,
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_applicationId = applicationId.ToGuidAsString(),
            invln_deliveryPhaseId = deliveryPhaseId.ToGuidAsString(),
            invln_fieldstoretrieve = DeliveryPhaseCrmFields,
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task Remove(string applicationId, string deliveryPhaseId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var request = new invln_deletedeliveryphaseRequest
        {
            invln_userId = userId,
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_applicationId = applicationId.ToGuidAsString(),
            invln_deliveryPhaseId = deliveryPhaseId.ToGuidAsString(),
        };

        await _service.ExecuteAsync<invln_deletedeliveryphaseRequest, invln_deletedeliveryphaseResponse>(
            request,
            x => x.ResponseName,
            cancellationToken);
    }

    public async Task<string> Save(DeliveryPhaseDto deliveryPhase, string organisationId, string userId, CancellationToken cancellationToken)
    {
        var request = new invln_setdeliveryphaseRequest
        {
            invln_organisationId = organisationId.TryToGuidAsString(),
            invln_userId = userId,
            invln_applicationId = deliveryPhase.applicationId.ToGuidAsString(),
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
