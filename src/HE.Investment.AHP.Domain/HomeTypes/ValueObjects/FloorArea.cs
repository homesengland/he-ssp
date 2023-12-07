using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class FloorArea : ValueObject
{
    private const decimal MinValue = 0;

    private const decimal MaxValue = 999.99m;

    public FloorArea(string? value)
    {
        if (!decimal.TryParse(value!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(FloorArea), ValidationErrorMessage.SquareMetersMustBeNumber())
                .CheckErrors();
        }

        if (decimal.Round(parsedValue, 2) != parsedValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(FloorArea), ValidationErrorMessage.SquareMetersMustBeNumber())
                .CheckErrors();
        }

        if (parsedValue is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(FloorArea), ValidationErrorMessage.SquareMetersMustBeNumber())
                .CheckErrors();
        }

        Value = parsedValue;
    }

    public FloorArea(decimal value)
    {
        if (value is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(FloorArea), ValidationErrorMessage.SquareMetersMustBeNumber())
                .CheckErrors();
        }

        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
