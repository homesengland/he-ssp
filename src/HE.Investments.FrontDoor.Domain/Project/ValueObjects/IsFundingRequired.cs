using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class IsFundingRequired : ValueObject, IQuestion
{
    public IsFundingRequired(bool? isFundingRequired)
    {
        if (isFundingRequired.IsProvided())
        {
            Value = isFundingRequired!.Value;
        }
        else
        {
            OperationResult.ThrowValidationError(nameof(IsFundingRequired), "Select yes if you require funding for your project");
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
