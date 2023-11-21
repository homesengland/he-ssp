using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class RCGFContribution : ValueObject
{
    public RCGFContribution(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.RCGFContribution, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static RCGFContribution? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new RCGFContribution(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
