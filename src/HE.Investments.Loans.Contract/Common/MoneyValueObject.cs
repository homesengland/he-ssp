using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Common;

public abstract class MoneyValueObject : ValueObject
{
    private const long MinValue = 0;

    private const long MaxValue = 999999999;

    protected MoneyValueObject(
        string value,
        string fieldName,
        string displayName,
        long minValue = MinValue,
        long maxValue = MaxValue)
    {
        _ = decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);

        if (parsedValue.ToString(CultureInfo.InvariantCulture) != value)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue));
        }

        if (decimal.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) > maxValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue));
        }
        else if (decimal.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) < minValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue));
        }

        Value = parsedValue;
    }

    protected MoneyValueObject(
        decimal value,
        string fieldName,
        string displayName,
        long minValue = MinValue,
        long maxValue = MaxValue)
    {
        if (value > maxValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue));
        }
        else if (value < minValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue));
        }

        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
