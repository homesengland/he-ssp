using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ProjectInfrastructure : ValueObject, IQuestion
{
    public ProjectInfrastructure(IList<InfrastructureType> infrastructureTypes)
    {
        if (!infrastructureTypes.Any())
        {
            OperationResult.New()
                .AddValidationError("Infrastructure", "Select the infrastructure your project delivers, or select 'I do not know'")
                .CheckErrors();
        }

        if (infrastructureTypes.Any(x => x == InfrastructureType.IDoNotKnow) && infrastructureTypes.Count > 1)
        {
            OperationResult.New()
                .AddValidationError("Infrastructure", ValidationErrorMessage.ExclusiveOptionSelected("infrastructure type", "I do not know"))
                .CheckErrors();
        }

        Values = infrastructureTypes;
    }

    public ProjectInfrastructure()
    {
        Values = Array.Empty<InfrastructureType>();
    }

    public IList<InfrastructureType> Values { get; }

    public static ProjectInfrastructure Empty() => new();

    public bool IsAnswered()
    {
        return Values.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        return Values.Cast<object?>();
    }
}
