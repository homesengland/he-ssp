using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhasesEntity
{
    private readonly IList<DeliveryPhaseEntity> _deliveryPhases;

    private readonly IList<DeliveryPhaseEntity> _toRemove = new List<DeliveryPhaseEntity>();

    private readonly ApplicationBasicInfo _application;

    public DeliveryPhasesEntity(ApplicationBasicInfo application, IEnumerable<DeliveryPhaseEntity> deliveryPhases, SectionStatus status)
    {
        _application = application;
        _deliveryPhases = deliveryPhases.ToList();
        Status = status;
    }

    public ApplicationId ApplicationId => _application.Id;

    public ApplicationName ApplicationName => _application.Name;

    public IEnumerable<IDeliveryPhaseEntity> DeliveryPhases => _deliveryPhases;

    public IEnumerable<IDeliveryPhaseEntity> ToRemove => _toRemove;

    public SectionStatus Status { get; private set; }

    public void Add(DeliveryPhaseEntity entity)
    {
        _deliveryPhases.Add(entity);
    }

    public void Remove(DeliveryPhaseId deliveryPhaseId, RemoveDeliveryPhaseAnswer removeAnswer)
    {
        if (removeAnswer == RemoveDeliveryPhaseAnswer.Undefined)
        {
            OperationResult.New().AddValidationError(nameof(RemoveDeliveryPhaseAnswer), "Select whether you want to remove this delivery phase").CheckErrors();
        }

        if (removeAnswer == RemoveDeliveryPhaseAnswer.Yes)
        {
            var deliveryPhase = GetEntityById(deliveryPhaseId);

            _toRemove.Add(deliveryPhase);
            _deliveryPhases.Remove(deliveryPhase);
        }
    }

    public IDeliveryPhaseEntity GetById(DeliveryPhaseId deliveryPhaseId) => GetEntityById(deliveryPhaseId);

    private DeliveryPhaseEntity GetEntityById(DeliveryPhaseId deliveryPhaseId) => _deliveryPhases.SingleOrDefault(x => x.Id == deliveryPhaseId)
                                                                                  ?? throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
}
