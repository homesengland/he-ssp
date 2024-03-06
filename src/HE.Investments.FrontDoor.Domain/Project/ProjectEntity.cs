using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public ProjectEntity(
        FrontDoorProjectId id,
        string name,
        ProjectAffordableHomesAmount? affordableHomesAmount = null,
        ProjectInfrastructureTypes? infrastructureTypes = null)
    {
        Id = id;
        Name = name;
        AffordableHomesAmount = affordableHomesAmount ?? ProjectAffordableHomesAmount.Empty();
        InfrastructureTypes = infrastructureTypes ?? ProjectInfrastructureTypes.Empty();
    }

    public FrontDoorProjectId Id { get; private set; }

    public string Name { get; private set; }

    public ProjectAffordableHomesAmount AffordableHomesAmount { get; private set; }

    public ProjectInfrastructureTypes InfrastructureTypes { get; private set; }

    public static ProjectEntity New(string name) => new(FrontDoorProjectId.New(), name);

    public void ProvideName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            OperationResult.New().AddValidationError("Name", "Enter name").CheckErrors();
        }

        Name = name!;
    }

    public void ProvideAffordableHomesAmount(ProjectAffordableHomesAmount affordableHomesAmount)
    {
        AffordableHomesAmount = _modificationTracker.Change(AffordableHomesAmount, affordableHomesAmount);
    }

    public void ProvideInfrastructureTypes(ProjectInfrastructureTypes infrastructureTypes)
    {
        InfrastructureTypes = _modificationTracker.Change(InfrastructureTypes, infrastructureTypes);
    }

    public void SetId(FrontDoorProjectId newId)
    {
        if (!Id.IsNew)
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
    }
}
