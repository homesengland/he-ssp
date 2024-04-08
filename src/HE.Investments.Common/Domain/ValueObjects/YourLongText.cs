using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class YourLongText : StringValueObject
{
    protected YourLongText(string? value, string fieldName, string fieldDisplayName)
        : base(
            value,
            fieldName,
            ValidationErrorMessage.MustProvideYourRequiredField(fieldDisplayName),
            ValidationErrorMessage.YourStringLengthExceeded(fieldDisplayName, MaximumInputLength.LongInput),
            MaximumInputLength.LongInput)
    {
    }
}
