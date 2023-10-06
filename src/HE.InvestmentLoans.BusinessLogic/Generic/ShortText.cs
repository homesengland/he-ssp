using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Generic;
public class ShortText : ValueObject
{
    public ShortText(string? value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(ShortText), GenericValidationError.NoValueProvided)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult.New()
                .AddValidationError(nameof(ShortText), GenericValidationError.TextTooLong)
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
