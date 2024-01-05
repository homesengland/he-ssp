using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

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

        value = value!.Trim();
        if (value.Length > MaximumInputLength.ShortInput)
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
