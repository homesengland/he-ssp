using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class PlanningReferenceNumber : ValueObject
{
    public PlanningReferenceNumber(bool exists, string? value)
    {
        Exists = exists;

        if (Exists && value.IsProvided() && value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(PlanningReferenceNumber), ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.PlanningReferenceNumber))
                .CheckErrors();
        }

        if (Exists)
        {
            Value = value;
        }
    }

    public bool Exists { get; }

    public string? Value { get; }

    public static PlanningReferenceNumber FromString(string existsString, string? value)
    {
        return new PlanningReferenceNumber(existsString.MapToNonNullableBool(), value);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Exists;
    }
}
