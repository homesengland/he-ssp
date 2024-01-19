using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public class DateValidator
{
    private readonly string? _day;
    private readonly string? _month;
    private readonly string? _year;
    private readonly string _fieldName;
    private readonly string _fieldLabel;
    private readonly OperationResult _operationResult;
    private DateTime? _parsedValue;

    private DateValidator(string? day, string? month, string? year, string fieldName, string fieldLabel, OperationResult operationResult)
    {
        _day = day;
        _month = month;
        _year = year;
        _fieldName = fieldName;
        _fieldLabel = fieldLabel;
        _operationResult = operationResult;
    }

    public static implicit operator DateTime?(DateValidator v) => v._parsedValue ?? null;

    public static DateValidator For(string? day, string? month, string? year, string fieldName, string fieldLabel, OperationResult? operationResult = null)
    {
        return new DateValidator(day, month, year, fieldName, fieldLabel, operationResult ?? OperationResult.New());
    }

    public DateValidator IsProvided(string? errorMessage = null)
    {
        if (string.IsNullOrWhiteSpace(_day) ||
            string.IsNullOrWhiteSpace(_month) ||
            string.IsNullOrWhiteSpace(_year))
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterDate);
            return this;
        }

        return this;
    }

    public DateValidator IsValid(string? errorMessage = null)
    {
        if (string.IsNullOrWhiteSpace(_day) &&
            string.IsNullOrWhiteSpace(_month) &&
            string.IsNullOrWhiteSpace(_year))
        {
            return this;
        }

        if (int.TryParse(_day, out var day) &&
            int.TryParse(_month, out var month) &&
            int.TryParse(_year, out var year))
        {
            try
            {
                _parsedValue = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
            }
            catch (ArgumentOutOfRangeException)
            {
                AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustBeDate(_fieldLabel));
            }

            return this;
        }

        AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustBeDate(_fieldLabel));
        return this;
    }

    private void AddError(string fieldName, string? errorMessage = null)
    {
        if (!_operationResult.HasValidationErrors)
        {
            _operationResult.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
        }
    }
}
