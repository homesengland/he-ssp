using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.Contract.Common;

public class LongText : ValueObject
{
    public LongText(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(LongText), GenericValidationError.NoValueProvided)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(nameof(LongText), GenericValidationError.TextTooLong)
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
