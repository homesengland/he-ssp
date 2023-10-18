using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.Contract.Common;

public class ShortText : ValueObject
{
    public ShortText(string? value, string fieldName = nameof(ShortText))
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, GenericValidationError.NoValueProvided)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult.New()
                .AddValidationError(fieldName, GenericValidationError.TextTooLong)
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
