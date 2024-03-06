using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class StringValueObject : ValueObject
{
    protected StringValueObject(
        string? value,
        string fieldName,
        string noValueProvidedErrorMessage,
        string textTooLongErrorMessage,
        int maxLength)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, noValueProvidedErrorMessage)
                .CheckErrors();
        }

        value = value!.Trim();
        if (value.Length > maxLength)
        {
            OperationResult.New()
                .AddValidationError(fieldName, textTooLongErrorMessage)
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
