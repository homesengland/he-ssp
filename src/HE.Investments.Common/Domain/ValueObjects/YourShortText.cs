using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class YourShortText : StringValueObject
{
    protected YourShortText(string? value, string fieldName, string fieldDisplayName)
        : base(
            value,
            fieldName,
            ValidationErrorMessage.MustProvideYourRequiredField(fieldDisplayName),
            ValidationErrorMessage.YourStringLengthExceeded(fieldDisplayName, MaximumInputLength.ShortInput),
            MaximumInputLength.ShortInput)
    {
    }
}
