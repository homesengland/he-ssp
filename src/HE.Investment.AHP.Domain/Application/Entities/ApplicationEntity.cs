using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity : DomainEntity
{
    private readonly ModificationTracker _statusModificationTracker = new();

    private readonly ModificationTracker _modificationTracker = new();

    private readonly ApplicationState _applicationState;

    public ApplicationEntity(
        SiteId siteId,
        AhpApplicationId id,
        ApplicationName name,
        ApplicationStatus status,
        ApplicationReferenceNumber referenceNumber,
        ApplicationTenure? tenure,
        AuditEntry? lastModified,
        ApplicationSections sections,
        IApplicationStateFactory applicationStateFactory)
    {
        SiteId = siteId;
        Id = id;
        Name = name;
        Status = status;
        ReferenceNumber = referenceNumber;
        Tenure = tenure;
        LastModified = lastModified;
        Sections = sections;
        _applicationState = applicationStateFactory.Create(status);
    }

    public SiteId SiteId { get; }

    public AhpApplicationId Id { get; private set; }

    public ApplicationName Name { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public IEnumerable<AhpApplicationOperation> AllowedOperations => _applicationState.AllowedOperations;

    public string? ChangeStatusReason { get; private set; }

    public ApplicationReferenceNumber ReferenceNumber { get; }

    public ApplicationTenure? Tenure { get; private set; }

    public AuditEntry? LastModified { get; }

    public ApplicationSections Sections { get; }

    public bool IsModified => _modificationTracker.IsModified;

    public bool IsStatusModified => _statusModificationTracker.IsModified;

    public bool IsNew => Id.IsNew;

    public static ApplicationEntity New(SiteId siteId, ApplicationName name, ApplicationTenure tenure, IApplicationStateFactory applicationStateFactory) => new(
        siteId,
        AhpApplicationId.New(),
        name,
        ApplicationStatus.New,
        new ApplicationReferenceNumber(null),
        tenure,
        null,
        new ApplicationSections(new List<ApplicationSection>()),
        applicationStateFactory);

    public void SetId(AhpApplicationId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public void AreAllSectionsCompleted()
    {
        if (Sections.Sections.Any(s => s.Status != SectionStatus.Completed))
        {
            var operationResult = OperationResult.New().AddValidationError("Status", "Cannot submit application with at least one not completed section.");
            throw new DomainValidationException(operationResult);
        }
    }

    public void Submit(RepresentationsAndWarranties reason)
    {
        AreAllSectionsCompleted();

        Status = _statusModificationTracker.Change(Status, _applicationState.Trigger(AhpApplicationOperation.Submit));
        ChangeStatusReason = reason.Value;
    }

    public void Hold(HoldReason reason)
    {
        Status = _statusModificationTracker.Change(Status, _applicationState.Trigger(AhpApplicationOperation.PutOnHold));
        ChangeStatusReason = reason.Value;

        Publish(new ApplicationHasBeenPutOnHoldEvent(Id));
    }

    public void Reactivate()
    {
        Status = _statusModificationTracker.Change(Status, _applicationState.Trigger(AhpApplicationOperation.Reactivate));
    }

    public void RequestToEdit(RequestToEditReason reason)
    {
        Status = _statusModificationTracker.Change(Status, _applicationState.Trigger(AhpApplicationOperation.RequestToEdit));
        ChangeStatusReason = reason.Value;

        Publish(new ApplicationHasBeenRequestedToEditEvent(Id));
    }

    public void Withdraw(WithdrawReason reason)
    {
        Status = _statusModificationTracker.Change(Status, _applicationState.Trigger(AhpApplicationOperation.Withdraw));
        ChangeStatusReason = reason.Value;

        Publish(new ApplicationHasBeenWithdrawnEvent(Id));
    }
}
