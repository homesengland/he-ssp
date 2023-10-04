namespace HE.InvestmentLoans.Contract.User.ValueObjects;

using System.Runtime.Serialization;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

[Serializable]
public class SecondaryTelephoneNumber : ValueObject, ISerializable
{
    protected SecondaryTelephoneNumber(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        Value = info.GetString(nameof(Value)) ?? string.Empty;
    }

    private SecondaryTelephoneNumber(string value)
    {
        if (value?.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.SecondaryTelephoneNumber),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.SecondaryTelephoneNumber))
                .CheckErrors();
        }

        Value = value!;
    }

    private string Value { get; }

    public static SecondaryTelephoneNumber New(string value) => new(value);

    public static SecondaryTelephoneNumber? FromString(string? secondaryTelephoneNumber)
    {
        if (string.IsNullOrEmpty(secondaryTelephoneNumber))
        {
            return null;
        }

        return new SecondaryTelephoneNumber(secondaryTelephoneNumber);
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        info.AddValue("Value", Value);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
