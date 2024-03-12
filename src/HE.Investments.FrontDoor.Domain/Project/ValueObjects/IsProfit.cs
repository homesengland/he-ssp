using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class IsProfit : ValueObject, IQuestion
{
    public IsProfit(bool? isFundingRequired)
    {
        if (isFundingRequired.IsProvided())
        {
            Value = isFundingRequired!.Value;
        }
        else
        {
            OperationResult.ThrowValidationError(nameof(IsProfit), "Select yes if there is an intention to make a profit from this project");
        }
    }

    private IsProfit()
    {
    }

    public static IsProfit Empty => new();

    public bool? Value { get; }

    public bool IsAnswered()
    {
        return Value.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
