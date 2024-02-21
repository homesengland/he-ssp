using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ApplicationEntity(
        SiteId siteId,
        AhpApplicationId id,
        ApplicationName name,
        ApplicationStatus status,
        ApplicationReferenceNumber referenceNumber,
        ApplicationTenure? tenure,
        AuditEntry? lastModified,
        ApplicationSections sections)
    {
        SiteId = siteId;
        Id = id;
        Name = name;
        Status = status;
        ReferenceNumber = referenceNumber;
        Tenure = tenure;
        LastModified = lastModified;
        Sections = sections;
    }

    public SiteId SiteId { get; }

    public AhpApplicationId Id { get; private set; }

    public ApplicationName Name { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public WithdrawReason? WithdrawReason { get; private set; }

    public HoldReason? HoldReason { get; private set; }

    public RequestToEditReason? RequestToEditReason { get; private set; }

    public ApplicationReferenceNumber ReferenceNumber { get; }

    public ApplicationTenure? Tenure { get; private set; }

    public AuditEntry? LastModified { get; }

    public ApplicationSections Sections { get; }

    public RepresentationsAndWarranties RepresentationsAndWarranties { get; private set; }

    public bool IsModified => _modificationTracker.IsModified;

    public bool IsNew => Id.IsNew;

    public static ApplicationEntity New(SiteId siteId, ApplicationName name, ApplicationTenure tenure) => new(
        siteId,
        AhpApplicationId.New(),
        name,
        ApplicationStatus.New,
        new ApplicationReferenceNumber(null),
        tenure,
        null,
        new ApplicationSections(new List<ApplicationSection>()));

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

    public async Task Submit(IChangeApplicationStatus applicationSubmit, OrganisationId organisationId, RepresentationsAndWarranties representationsAndWarranties, CancellationToken cancellationToken)
    {
        AreAllSectionsCompleted();

        Status = _modificationTracker.Change(Status, ApplicationStatus.ApplicationSubmitted);
        RepresentationsAndWarranties = _modificationTracker.Change(RepresentationsAndWarranties, representationsAndWarranties);

        await applicationSubmit.ChangeApplicationStatus(this, organisationId, null, cancellationToken); // todo this will be change to pass representationsAndWarranties when it will be added to crm
    }

    public async Task Hold(IChangeApplicationStatus applicationHold, HoldReason? newHoldReason, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.OnHold);
        HoldReason = _modificationTracker.Change(HoldReason, newHoldReason);

        await applicationHold.ChangeApplicationStatus(this, organisationId, HoldReason?.Value, cancellationToken);

        Publish(new ApplicationHasBeenPutOnHoldEvent(Id));
    }

    public async Task Reactivate(IChangeApplicationStatus applicationReactivate, ApplicationStatus newApplicationStatus, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        Status = _modificationTracker.Change(Status, newApplicationStatus);

        await applicationReactivate.ChangeApplicationStatus(this, organisationId, null, cancellationToken);
    }

    public async Task RequestToEdit(IChangeApplicationStatus applicationRequestToEdit, RequestToEditReason? newRequestToEditReason, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.RequestedEditing);
        RequestToEditReason = _modificationTracker.Change(RequestToEditReason, newRequestToEditReason);

        await applicationRequestToEdit.ChangeApplicationStatus(this, organisationId, RequestToEditReason?.Value, cancellationToken);

        Publish(new ApplicationHasBeenRequestedToEditEvent(Id));
    }

    public async Task Withdraw(IChangeApplicationStatus applicationWithdraw, WithdrawReason? newWithdrawReason, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var statusesAfterSubmit = ApplicationStatusDivision.GetAllStatusesAllowedToChangeApplicationStatusToWithdrawn();
        WithdrawReason = _modificationTracker.Change(WithdrawReason, newWithdrawReason);

        if (Status == ApplicationStatus.Draft)
        {
            Status = _modificationTracker.Change(Status, ApplicationStatus.Deleted);
            await applicationWithdraw.ChangeApplicationStatus(this, organisationId, WithdrawReason?.Value, cancellationToken);
        }
        else if (statusesAfterSubmit.Contains(Status))
        {
            Status = _modificationTracker.Change(Status, ApplicationStatus.Withdrawn);
            await applicationWithdraw.ChangeApplicationStatus(this, organisationId, WithdrawReason?.Value, cancellationToken);
        }
        else
        {
            throw new DomainException("The application cannot be withdrawn", CommonErrorCodes.ApplicationCannotBeWithdrawn);
        }

        Publish(new ApplicationHasBeenWithdrawnEvent(Id));
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(Status);
    }
}
