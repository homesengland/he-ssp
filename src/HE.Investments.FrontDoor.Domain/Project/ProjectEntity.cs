using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.FrontDoor.Contract.Project;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    public ProjectEntity(FrontDoorProjectId id, string name)
    {
        Id = id;
        Name = name;
    }

    public FrontDoorProjectId Id { get; private set; }

    public string Name { get; private set; }

    public static ProjectEntity New(string name) => new(FrontDoorProjectId.New(), name);

    public void ProvideName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            OperationResult.New().AddValidationError("Name", "Enter name").CheckErrors();
        }

        Name = name!;
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
