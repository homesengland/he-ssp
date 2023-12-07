using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor", Justification = "Required for ValueObject")]
public abstract class PoundsValueObject : ValueObject
{
    protected PoundsValueObject(decimal value, UiFields uiFields)
    {
        UiFields = uiFields;
        Value = WholeNumberValidator.Validate(value, UiFields.FieldName, DisplayName, ValidationErrorMessage.WholePoundInput(DisplayName));
    }

    protected PoundsValueObject(string value, UiFields uiFields, string? invalidValueValidationMessage = null)
    {
        UiFields = uiFields;
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (!decimal.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(UiFields.FieldName, invalidValueValidationMessage ?? ValidationErrorMessage.WholePoundInput(DisplayName))
                .CheckErrors();
        }

        Value = WholeNumberValidator.Validate(
            parsedValue,
            UiFields.FieldName,
            DisplayName,
            invalidValueValidationMessage ?? ValidationErrorMessage.WholePoundInput(DisplayName));
    }

    public UiFields UiFields { get; }

    public decimal Value { get; }

    private string DisplayName => UiFields.DisplayName ?? GenericMessages.PoundFieldDefaultName;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
