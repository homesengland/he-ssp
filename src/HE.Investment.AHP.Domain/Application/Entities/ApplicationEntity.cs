using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ApplicationEntity(
        AhpApplicationId id,
        ApplicationName name,
        ApplicationStatus status,
        ApplicationReferenceNumber referenceNumber,
        ApplicationTenure? tenure,
        AuditEntry? lastModified,
        ApplicationSections sections)
    {
        Id = id;
        Name = name;
        Status = status;
        ReferenceNumber = referenceNumber;
        Tenure = tenure;
        LastModified = lastModified;
        Sections = sections;
    }

    public AhpApplicationId Id { get; private set; }

    public ApplicationName Name { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public ChangeStatusReason? ChangeStatusReason { get; private set; }

    public ApplicationReferenceNumber ReferenceNumber { get; }

    public ApplicationTenure? Tenure { get; private set; }

    public AuditEntry? LastModified { get; }

    public ApplicationSections Sections { get; }

    public bool IsModified => _modificationTracker.IsModified;

    public bool IsNew => Id.IsNew;

    public static ApplicationEntity New(ApplicationName name, ApplicationTenure tenure) => new(
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

    public void Submit()
    {
        if (Sections.Sections.Any(s => s.Status != SectionStatus.Completed))
        {
            var operationResult = OperationResult.New().AddValidationError("Status", "Cannot submit application with at least one not completed section.");
            throw new DomainValidationException(operationResult);
        }

        Status = _modificationTracker.Change(Status, ApplicationStatus.ApplicationSubmitted);
    }

    public void Hold()
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.OnHold);
    }

    public void Withdraw()
    {
        Status = _modificationTracker.Change(Status, ApplicationStatus.Withdrawn);
    }

    public void ProvideChangeStatusReason(ChangeStatusReason? newChangeStatusReason)
    {
        ChangeStatusReason = _modificationTracker.Change(ChangeStatusReason, newChangeStatusReason);
    }
}
