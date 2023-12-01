using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class NumericValueObject : ValueObject
{
    public NumericValueObject(string fieldName, string value, string validationErrorMsg)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, fieldName, operationResult)
            .IsWholeNumber(validationErrorMsg)
            .IsBetween(1, 999999999, validationErrorMsg);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
