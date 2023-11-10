using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public abstract class RequiredStringValueObject : ValueObject
{
    protected RequiredStringValueObject(string? value, string fieldName, string displayName, int maxLength)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MissingRequiredField(displayName))
                .CheckErrors();
        }

        if (value!.Length > maxLength)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.StringLengthExceeded(displayName, maxLength))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
