using HE.Investments.Common.Contract.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class DateValueObject : ValueObject
{
    protected DateValueObject(
        string? day,
        string? month,
        string? year,
        string fieldName,
        string fieldDescription,
        bool isEmpty = false)
    {
        if (isEmpty)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
        {
            OperationResult.ThrowValidationError(fieldName, $"Enter when {fieldDescription}");
        }

        var missingParts = new[]
            {
                string.IsNullOrWhiteSpace(day) ? "day" : null,
                string.IsNullOrWhiteSpace(month) ? "month" : null,
                string.IsNullOrWhiteSpace(year) ? "year" : null,
            }.Where(x => x != null)
            .ToList();

        if (missingParts.Count != 0)
        {
            OperationResult.ThrowValidationError(fieldName, $"The date when {fieldDescription} must include a {string.Join(" and ", missingParts)}");
        }

        var value = CreateDateTime(day, month, year);
        if (value == null)
        {
            OperationResult.ThrowValidationError(fieldName, $"When {fieldDescription} must be a real date");
        }

        Value = value!.Value;
    }

    protected DateValueObject(int day, int month, int year)
    {
        Value = new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Unspecified);
    }

    protected DateValueObject(DateTime value)
    {
        Value = value;
    }

    public DateTime Value { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private static DateTime? CreateDateTime(string? day, string? month, string? year)
    {
        if (int.TryParse(day, out var intDay)
            && int.TryParse(month, out var intMonth)
            && int.TryParse(year, out var intYear)
            && intYear is >= 1900 and <= 9999)
        {
            try
            {
                return new DateTime(intYear, intMonth, intDay, 0, 0, 0, 0, DateTimeKind.Unspecified);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        return null;
    }
}
