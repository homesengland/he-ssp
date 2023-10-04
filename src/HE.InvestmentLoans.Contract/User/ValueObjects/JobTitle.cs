namespace HE.InvestmentLoans.Contract.User.ValueObjects;

using System.Runtime.Serialization;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

[Serializable]
public class JobTitle : ValueObject, ISerializable
{
    protected JobTitle(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        Value = info.GetString(nameof(Value)) ?? string.Empty;
    }

    private JobTitle(string value)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(UserDetailsViewModel.JobTitle), ValidationErrorMessage.EnterJobTitle)
                .CheckErrors();
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.JobTitle),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.JobTitle))
                .CheckErrors();
        }

        Value = value;
    }

    private string Value { get; }

    public static JobTitle New(string value) => new(value);

    public static JobTitle? FromString(string? jobTitle)
    {
        if (string.IsNullOrEmpty(jobTitle))
        {
            return null;
        }

        return new JobTitle(jobTitle);
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
