using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class IsSupportRequired : ValueObject, IQuestion
{
    public IsSupportRequired(bool? isSupportRequired)
    {
        if (isSupportRequired.IsProvided())
        {
            Value = isSupportRequired!.Value;
        }
        else
        {
            OperationResult.ThrowValidationError(nameof(IsSupportRequired), "Select yes if your project would progress more slowly or stall without Homes England funding");
        }
    }

    public bool Value { get; }

    public bool IsAnswered()
    {
        return Value.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
