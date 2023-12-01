using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Entities;

public class ApplicationEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ApplicationEntity(
        ApplicationId id,
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

    public ApplicationId Id { get; private set; }

    public ApplicationName Name { get; private set; }

    public ApplicationStatus Status { get; private set; }

    public ApplicationReferenceNumber ReferenceNumber { get; }

    public ApplicationTenure? Tenure { get; private set; }

    public AuditEntry? LastModified { get; }

    public ApplicationSections Sections { get; }

    public bool IsModified => _modificationTracker.IsModified;

    public bool IsNew => Id.IsEmpty();

    public static ApplicationEntity New(ApplicationName name) => new(
        ApplicationId.Empty(),
        name,
        ApplicationStatus.New,
        new ApplicationReferenceNumber(null),
        null,
        null,
        new ApplicationSections(new List<ApplicationSection>()));

    public void SetId(ApplicationId newId)
    {
        if (!Id.IsEmpty())
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    public void ChangeName(ApplicationName name)
    {
        Name = _modificationTracker.Change(Name, name);
    }

    public void ChangeTenure(ApplicationTenure tenure)
    {
        Tenure = _modificationTracker.Change(Tenure, tenure);
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
}
