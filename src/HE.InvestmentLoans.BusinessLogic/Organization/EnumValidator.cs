using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization;

public class EnumValidator<TEnum>
    where TEnum : struct
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
    public static EnumValidator<TEnum> For(string? input, string fieldName, OperationResult? operationResult = null)
    {
        var result = operationResult ?? OperationResult.New();
        if (!Enum.TryParse<TEnum>(input, true, out var @enum))
        {
            result.AddValidationError(fieldName, ValidationErrorMessage.InvalidValue);
        }

        return new EnumValidator<TEnum>(@enum, fieldName, result);
    }
}
