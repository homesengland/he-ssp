namespace HE.InvestmentLoans.Contract.User.ValueObjects;

using System.Runtime.Serialization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

[Serializable]
public class TelephoneNumber : ValueObject, ISerializable
{
    protected TelephoneNumber(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        Value = info.GetString(nameof(Value)) ?? string.Empty;
    }

    private TelephoneNumber(string value)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(UserDetailsViewModel.TelephoneNumber), ValidationErrorMessage.EnterTelephoneNumber)
                .CheckErrors();
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.TelephoneNumber),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.TelephoneNumber))
                .CheckErrors();
        }

        Value = value;
    }

    private string Value { get; }

    public static TelephoneNumber New(string value) => new(value);

    public static TelephoneNumber? FromString(string? telephoneNumber)
    {
        if (string.IsNullOrEmpty(telephoneNumber))
        {
            return null;
        }

        return new TelephoneNumber(telephoneNumber);
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
