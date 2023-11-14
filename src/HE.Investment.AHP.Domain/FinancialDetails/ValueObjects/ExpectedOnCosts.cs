using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedOnCosts : ValueObject
{
    public ExpectedOnCosts(string value)
    {
        if (!int.TryParse(value, out var onCostInt) || onCostInt < 0 || onCostInt > 999999999)
        {
            OperationResult.New()
            .AddValidationError(FinancialDetailsValidationFieldNames.ExpectedOnCosts, FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
            .CheckErrors();
        }
        else
        {
            Value = onCostInt;
        }
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
