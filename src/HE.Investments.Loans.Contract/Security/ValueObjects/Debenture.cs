using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Security.ValueObjects;

public class Debenture : ValueObject
{
    public Debenture(string holderName, bool exists)
    {
        if (exists && holderName.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError("ChargesDebtCompanyInfo", ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
        }

        if (exists && holderName.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError("ChargesDebtCompanyInfo", ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.Debenture))
                .CheckErrors();
        }

        Holder = exists ? holderName : string.Empty;
        Exists = exists;
    }

    public string Holder { get; }

    public bool Exists { get; }

    public static Debenture FromString(string exists, string holder)
    {
        return new Debenture(holder, exists.MapToBool()!.Value);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Holder;
        yield return Exists;
    }
}
