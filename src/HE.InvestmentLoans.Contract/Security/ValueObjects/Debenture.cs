using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;
using Microsoft.VisualBasic;
using StackExchange.Redis;

namespace HE.InvestmentLoans.Contract.Security.ValueObjects;

public class Debenture : ValueObject
{
    public Debenture(string holderName, bool exists)
    {
        if (exists && holderName.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(Holder), ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
        }

        if (exists && holderName.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(nameof(Holder), ValidationErrorMessage.LongInputLengthExceeded)
                .CheckErrors();
        }

        Holder = holderName;
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
