using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.Contract.Common;

public class LongText : ValueObject
{
    public LongText(
        string value,
        string fieldName = nameof(LongText),
        string noValueProvidedErrorMessage = GenericValidationError.NoValueProvided,
        string textTooLongErrorMessage = GenericValidationError.TextTooLong)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, noValueProvidedErrorMessage)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
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
