using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DateValueObject : ValueObject
{
    public DateValueObject(string? day, string? month, string? year, string fieldName, string fieldLabel)
    {
        Build(day, month, year, fieldName, fieldLabel).CheckErrors();
    }

    protected DateValueObject(DateOnly value)
    {
        Value = value;
    }

    public DateOnly Value { get; private set; }

    public static bool ValuesProvided(string? day, string? month, string? year)
    {
        if (string.IsNullOrWhiteSpace(day) &&
            string.IsNullOrWhiteSpace(month) &&
            string.IsNullOrWhiteSpace(year))
        {
            return false;
        }

        return true;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? day, string? month, string? year, string fieldName, string fieldLabel)
    {
        var operationResult = OperationResult.New();

        DateTime? value = DateValidator
            .For(day, month, year, fieldName, fieldLabel, operationResult)
            .IsValid();

        if (value != null)
        {
            Value = new DateOnly(value.Value.Year, value.Value.Month, value.Value.Day);
        }

        return operationResult;
    }
}
