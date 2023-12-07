using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor", Justification = "Required for ValueObject")]
public abstract class PoundsValueObject : ValueObject
{
    protected PoundsValueObject(decimal value)
    {
        Value = PoundsValidator.Validate(value, UiFields.FieldName, UiFields.DisplayName);
    }

    protected PoundsValueObject(string value, string? invalidValueValidationMessage = null)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, ValidationErrorMessage.MissingRequiredField(UiFields.DisplayName))
                .CheckErrors();
        }

        if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, invalidValueValidationMessage ?? ValidationErrorMessage.PoundInput(UiFields.DisplayName))
                .CheckErrors();
        }

        Value = PoundsValidator.Validate(parsedValue, UiFields.FieldName, UiFields.DisplayName);
    }

    public abstract UiFields UiFields { get; }

    public decimal Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
