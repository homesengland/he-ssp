using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class LongText : StringValueObject
{
    protected LongText(string? value, string fieldName, string fieldDisplayName)
        : base(
            value,
            fieldName,
            ValidationErrorMessage.MustProvideRequiredField(fieldDisplayName),
            ValidationErrorMessage.StringLengthExceeded(fieldDisplayName, MaximumInputLength.LongInput),
            MaximumInputLength.LongInput)
    {
    }

    protected LongText(string? value, string fieldName, string noValueProvidedErrorMessage, string textTooLongErrorMessage)
        : base(value, fieldName, noValueProvidedErrorMessage, textTooLongErrorMessage, MaximumInputLength.LongInput)
    {
    }
}
