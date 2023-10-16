using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class AcceptHeTermsAndConditions : ValueObject
{
    public AcceptHeTermsAndConditions(bool value)
    {
        if (!value)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(AcceptHeTermsAndConditions), ValidationErrorMessage.AcceptTermsAndConditions)
                .CheckErrors();
        }

        Value = value!;
    }

    public bool Value { get; }

    public static AcceptHeTermsAndConditions? FromString(string? value)
    {
        return value == CommonResponse.Checked ? new AcceptHeTermsAndConditions(true) : new AcceptHeTermsAndConditions(false);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
