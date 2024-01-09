using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Common;

public class ShortText : ValueObject
{
    public ShortText(
        string? value,
        string fieldName = nameof(ShortText),
        string noValueProvidedErrorMessage = GenericValidationError.NoValueProvided,
        string textTooLongErrorMessage = GenericValidationError.TextTooLong)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, noValueProvidedErrorMessage)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult.New()
                .AddValidationError(fieldName, textTooLongErrorMessage)
                .CheckErrors();
        }

        Value = value;
    }

    public ShortText(string? value, string fieldName, string fieldDisplayName)
        : this(
            value,
            fieldName,
            ValidationErrorMessage.MissingRequiredField(fieldDisplayName),
            ValidationErrorMessage.ShortInputLengthExceeded(fieldDisplayName))
    {
    }

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
