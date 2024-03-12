using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class RequiredFunding : ValueObject, IQuestion
{
    public RequiredFunding(RequiredFundingOption? value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(RequiredFunding), "Select the amount of funding you require");
        }

        Value = value;
    }

    private RequiredFunding()
    {
    }

    public static RequiredFunding Empty => new();

    public RequiredFundingOption? Value { get; }

    public bool IsAnswered() => Value.HasValue;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
