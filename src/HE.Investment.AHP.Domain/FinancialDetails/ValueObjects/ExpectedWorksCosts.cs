using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedWorksCosts : ValueObject
{
    public ExpectedWorksCosts(string value)
    {
        var (isValid, intValue) = AmountValidator.Validate(value);
        if (!isValid || !intValue.HasValue)
        {
            OperationResult.New()
            .AddValidationError(FinancialDetailsValidationFieldNames.ExpectedWorksCosts, FinancialDetailsValidationErrors.InvalidExpectedWorksCosts)
            .CheckErrors();
        }
        else
        {
            Value = intValue.Value;
        }
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
