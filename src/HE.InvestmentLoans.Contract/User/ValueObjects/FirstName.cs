using System.Runtime.Serialization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

[Serializable]
public class FirstName : ValueObject, ISerializable
{
    protected FirstName(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        Value = info.GetString(nameof(Value)) ?? string.Empty;
    }

    private FirstName(string value)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(UserDetailsViewModel.FirstName), ValidationErrorMessage.EnterFirstName)
                .CheckErrors();
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.FirstName),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.FirstName))
                .CheckErrors();
        }

        Value = value;
    }

    private string Value { get; }

    public static FirstName New(string value) => new(value);

    public static FirstName? FromString(string? firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            return null;
        }

        return new FirstName(firstName);
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
