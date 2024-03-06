using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public class ShortText : StringValueObject
{
    protected ShortText(string? value, string fieldName, string fieldDisplayName)
        : base(
            value,
            fieldName,
            ValidationErrorMessage.MustProvideRequiredField(fieldDisplayName),
            ValidationErrorMessage.StringLengthExceeded(fieldDisplayName, MaximumInputLength.ShortInput),
            MaximumInputLength.ShortInput)
    {
    }
}
