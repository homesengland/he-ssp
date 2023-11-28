using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class CountyCouncilGrants : ValueObject
{
    public CountyCouncilGrants(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.CountyCouncilGrants, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static CountyCouncilGrants? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new CountyCouncilGrants(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
