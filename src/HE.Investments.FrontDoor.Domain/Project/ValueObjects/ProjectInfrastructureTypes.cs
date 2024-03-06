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
        if (infrastructureTypes.Any(x => x == InfrastructureType.IDoNotKnow) && infrastructureTypes.Count > 1)
        {
            OperationResult.New()
                .AddValidationError("InfrastructureTypes", ValidationErrorMessage.InvalidValue)
                .CheckErrors();
        }

        Values = infrastructureTypes;
    }

    public ProjectInfrastructureTypes()
        : this(Array.Empty<InfrastructureType>())
    {
    }

    public IEnumerable<InfrastructureType> Values { get; }

    public static ProjectInfrastructureTypes Empty() => new();

    public bool IsAnswered()
    {
        return Values.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Values;
    }
}
