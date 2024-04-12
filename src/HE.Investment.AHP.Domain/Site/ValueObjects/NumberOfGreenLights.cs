using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class NumberOfGreenLights : ValueObject
{
    private const string DisplayName = "value you enter for the Building for Life green traffic lights";

    private const int MinValue = 0;

    private const int MaxValue = 12;

    public NumberOfGreenLights(string value)
    {
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(NumberOfGreenLights), ValidationErrorMessage.MustBeWholeNumber(DisplayName))
                .CheckErrors();
        }

        if (parsedValue is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(NumberOfGreenLights), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
                .CheckErrors();
        }

        Value = parsedValue;
    }

    protected NumberOfGreenLights(int value)
    {
        if (value is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(NumberOfGreenLights), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
                .CheckErrors();
        }

        Value = value;
    }

    public int Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
