using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ProjectInfrastructureTypes : ValueObject, IQuestion
{
    public ProjectInfrastructureTypes(IList<InfrastructureType> infrastructureTypes)
    {
        if (!infrastructureTypes.Any())
        {
            OperationResult.New()
                .AddValidationError("InfrastructureTypes", "Select the infrastructure your project delivers, or select 'I do not know'")
                .CheckErrors();
        }

        if (infrastructureTypes.Any(x => x == InfrastructureType.IDoNotKnow) && infrastructureTypes.Count > 1)
        {
            OperationResult.New()
                .AddValidationError("InfrastructureTypes", ValidationErrorMessage.ExclusiveOptionSelected("infrastructure type", "I do not know"))
                .CheckErrors();
        }

        Values = infrastructureTypes;
    }

    public ProjectInfrastructureTypes()
        : this(Array.Empty<InfrastructureType>())
    {
    }

    public IEnumerable<InfrastructureType> Values { get; }

    public bool IsAnswered()
    {
        return Values.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        foreach (var infrastructureType in Values)
        {
            yield return infrastructureType;
        }
    }
}
