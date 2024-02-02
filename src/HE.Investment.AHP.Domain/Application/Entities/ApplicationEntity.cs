using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
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

    public ApplicationReferenceNumber ReferenceNumber { get; }

    public ApplicationTenure? Tenure { get; private set; }

    public AuditEntry? LastModified { get; }

    public ApplicationSections Sections { get; }

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

    public async Task Submit(IChangeApplicationStatus applicationSubmit, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (Sections.Sections.Any(s => s.Status != SectionStatus.Completed))
        {
            var operationResult = OperationResult.New().AddValidationError("Status", "Cannot submit application with at least one not completed section.");
            throw new DomainValidationException(operationResult);
        }

        Status = _modificationTracker.Change(Status, ApplicationStatus.ApplicationSubmitted);

        await applicationSubmit.ChangeApplicationStatus(this, organisationId, null, cancellationToken);
    }

    public async Task Hold(IChangeApplicationStatus applicationHold, HoldReason? newHoldReason, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.OnHold);
        HoldReason = _modificationTracker.Change(HoldReason, newHoldReason);

        await applicationHold.ChangeApplicationStatus(this, organisationId, HoldReason?.Value, cancellationToken);

        Publish(new ApplicationHasBeenPutOnHoldEvent(Id));
    }

    public async Task Withdraw(IChangeApplicationStatus applicationWithdraw, WithdrawReason? newWithdrawReason, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.Withdrawn);
        WithdrawReason = _modificationTracker.Change(WithdrawReason, newWithdrawReason);

        await applicationWithdraw.ChangeApplicationStatus(this, organisationId, WithdrawReason?.Value, cancellationToken);

        Publish(new ApplicationHasBeenWithdrawnEvent(Id));
    }
}
