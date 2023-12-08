using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor", Justification = "Required for ValueObject")]
public abstract class PoundsPenceValueObject : ValueObject
{
    protected PoundsPenceValueObject(decimal value)
    {
        Value = PoundsPencesValidator.Validate(value, UiFields.FieldName, DisplayName);
    }

    protected PoundsPenceValueObject(string value, string? invalidValueValidationMessage = null)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, invalidValueValidationMessage ?? ValidationErrorMessage.PoundInput(DisplayName))
                .CheckErrors();
        }

        Value = PoundsPencesValidator.Validate(parsedValue, UiFields.FieldName, DisplayName);
    }

    public abstract UiFields UiFields { get; }

    public decimal Value { get; }

    private string DisplayName => UiFields.DisplayName ?? GenericMessages.PoundFieldDefaultName;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
