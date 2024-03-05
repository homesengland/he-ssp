using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectEntity(
        FrontDoorProjectId id,
        string name,
        IList<SupportActivityType>? supportActivityTypes = null,
        ProjectAffordableHomesAmount? affordableHomesAmount = null)
    {
        Id = id;
        Name = name;
        SupportActivityTypes = supportActivityTypes ?? new List<SupportActivityType>();
        AffordableHomesAmount = affordableHomesAmount ?? ProjectAffordableHomesAmount.Empty();
    }

    public FrontDoorProjectId Id { get; private set; }

    public string Name { get; private set; }

    public ProjectAffordableHomesAmount AffordableHomesAmount { get; private set; }

    public IList<SupportActivityType> SupportActivityTypes { get; private set; }

    public static ProjectEntity New(string name) => new(FrontDoorProjectId.New(), name);

    public void ProvideName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            OperationResult.New().AddValidationError("Name", "Enter name").CheckErrors();
        }

        Name = name!;
    }

    public void ProvideSupportActivityTypes(IList<SupportActivityType> supportActivityTypes)
    {
        if (supportActivityTypes.Count == 0)
        {
            OperationResult.New().AddValidationError(nameof(SupportActivityTypes), "Select activities you require support for, or select ‘other'").CheckErrors();
        }

        SupportActivityTypes = _modificationTracker.Change(SupportActivityTypes, supportActivityTypes, null, SupportActivityTypesHaveChanged);
    }

    public bool IsTenureRequired(IList<SupportActivityType> supportActivityTypes) => supportActivityTypes.Count == 1 && supportActivityTypes.Contains(SupportActivityType.DevelopingHomes);

    public bool IsInfrastructureRequired(IList<SupportActivityType> supportActivityTypes) => supportActivityTypes.Count == 1 && supportActivityTypes.Contains(SupportActivityType.ProvidingInfrastructure);

    public void ProvideAffordableHomesAmount(ProjectAffordableHomesAmount affordableHomesAmount)
    {
        AffordableHomesAmount = _modificationTracker.Change(AffordableHomesAmount, affordableHomesAmount);
    }

    public void SetId(FrontDoorProjectId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }

    private void SupportActivityTypesHaveChanged(IList<SupportActivityType> newSupportActivityTypes)
    {
        if (!IsTenureRequired(newSupportActivityTypes))
        {
            AffordableHomesAmount = ProjectAffordableHomesAmount.Empty();
        }

        if (!IsInfrastructureRequired(newSupportActivityTypes))
        {
            // TODO: Wipe answers #91004: Assess infrastructure delivery (portal user)
        }
    }
}
