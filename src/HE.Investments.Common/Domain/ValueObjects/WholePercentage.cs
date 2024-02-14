using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public class WholePercentage : ValueObject
{
    public WholePercentage(decimal value, UiFields? uiFields = null)
    {
        uiFields ??= new UiFields("Value", "Value");
        if (value < 0)
        {
            OperationResult.ThrowValidationError(uiFields.FieldName, ValidationErrorMessage.PercentageInput(uiFields.DisplayName ?? "Value"));
        }

        Value = value;
    }

    public static WholePercentage Hundered => new(1m);

    public decimal Value { get; }

    public static WholePercentage FromString(string? value, UiFields? uiFields = null)
    {
        uiFields ??= new UiFields("Value", "Value");

        if (value.IsNotProvided())
        {
            OperationResult.ThrowValidationError(uiFields.FieldName, ValidationErrorMessage.MissingRequiredField(uiFields.DisplayName ?? "Value"));
        }

        if (!decimal.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.ThrowValidationError(uiFields.FieldName, ValidationErrorMessage.PercentageInput(uiFields.DisplayName ?? "Value"));
        }

        return new WholePercentage(parsedValue / 100m, uiFields);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
