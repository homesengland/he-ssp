using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public class LongText : ValueObject
{
    public LongText(
        string? value,
        string fieldName = nameof(LongText),
        string noValueProvidedErrorMessage = GenericValidationError.NoValueProvided,
        string textTooLongErrorMessage = GenericValidationError.TextTooLong)
    {
        var normalisedValue = value?.NormalizeLineEndings();

        if (normalisedValue.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, noValueProvidedErrorMessage)
                .CheckErrors();
        }

        if (normalisedValue!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(fieldName, textTooLongErrorMessage)
                .CheckErrors();
        }

        Value = normalisedValue;
    }

    public LongText(string value, string fieldName, string fieldDisplayName)
        : this(
            value,
            fieldName,
            ValidationErrorMessage.MissingRequiredField(fieldDisplayName),
            ValidationErrorMessage.LongInputLengthExceeded(fieldDisplayName))
    {
    }

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
