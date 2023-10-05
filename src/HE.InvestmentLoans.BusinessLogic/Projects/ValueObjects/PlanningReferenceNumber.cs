using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class PlanningReferenceNumber : ValueObject
{
    public PlanningReferenceNumber(bool exists, string value)
    {
        Exists = exists;

        if (Exists && value.IsProvided() && value.Length > MaximumInputLength.ShortInput)
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

    public string Value { get; }

    public static PlanningReferenceNumber FromString(string existsString, string value)
    {
        return new PlanningReferenceNumber(existsString.MapToNonNullableBool(), value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return Exists;
    }
}
