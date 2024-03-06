using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class IsSiteIdentified : ValueObject, IQuestion
{
    public IsSiteIdentified(bool? isSiteIdentified)
    {
        if (isSiteIdentified.IsProvided())
        {
            Value = isSiteIdentified!.Value;
        }
        else
        {
            OperationResult.ThrowValidationError(nameof(IsSiteIdentified), ValidationErrorMessage.SelectIdentifiedSite);
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
