using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using Newtonsoft.Json.Linq;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SharedOwnershipSales : ValueObject
{
    public SharedOwnershipSales(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.SharedOwnershipSales, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static SharedOwnershipSales? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new SharedOwnershipSales(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
