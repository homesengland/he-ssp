using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public class EnumValidator<TEnum>
    where TEnum : struct, Enum
{
    private readonly TEnum _value;

    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "for future use")]
    private readonly string _fieldName;

    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "for future use")]
    private readonly OperationResult _operationResult;

    private EnumValidator(TEnum value, string fieldName, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _operationResult = operationResult;
    }

    public static implicit operator TEnum(EnumValidator<TEnum> v) => v._value;

    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Accepted here")]
    public static EnumValidator<TEnum> Required(TEnum input, string fieldName, string? errorMessage = null, OperationResult? operationResult = null)
    {
        var result = operationResult ?? OperationResult.New();
        if (Convert.ToInt32(input, CultureInfo.InvariantCulture) == 0)
        {
            result.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
        }

        return new EnumValidator<TEnum>(input, fieldName, result);
    }
}
