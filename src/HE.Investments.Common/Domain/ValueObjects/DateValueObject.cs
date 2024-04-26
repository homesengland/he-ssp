using System.ComponentModel;
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

        var (dayFieldName, monthFieldName, yearFieldName) = DatePartFieldNamesProvider.Get(fieldName);

        if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
        {
            var operationResult = OperationResult.New();
            operationResult.AddValidationError(dayFieldName, $"Enter the {fieldDescription}");
            operationResult.AddValidationError(monthFieldName, string.Empty);
            operationResult.AddValidationError(yearFieldName, string.Empty);
            operationResult.CheckErrors();
        }

        var missingParts = new[]
            {
                string.IsNullOrWhiteSpace(day) ? (DisplayName: "day", FieldName: dayFieldName) : default,
                string.IsNullOrWhiteSpace(month) ? (DisplayName: "month", FieldName: monthFieldName) : default,
                string.IsNullOrWhiteSpace(year) ? (DisplayName: "year", FieldName: yearFieldName) : default,
            }.Where(x => x != default)
            .ToList();

        if (missingParts.Count != 0)
        {
            var missingPartsDisplayNames = missingParts.Select(ms => ms.DisplayName).ToArray();
            var operationResult = OperationResult.New();
            operationResult.AddValidationError(missingParts[0].FieldName, $"The {fieldDescription} must include a {string.Join(" and ", missingPartsDisplayNames)}");
            foreach (var (displayName, partFieldName) in missingParts.Skip(1))
            {
                operationResult.AddValidationError(partFieldName, string.Empty);
            }

            operationResult.CheckErrors();
        }

        var value = CreateDateTime(day, month, year);
        if (value == null)
        {
            OperationResult.ThrowValidationError(fieldName, $"The {fieldDescription} must be a real date");
        }

        Value = value!.Value;
    }

    protected DateValueObject(int day, int month, int year)
    {
        Value = new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Unspecified);
    }

    protected DateValueObject(DateTime? value)
    {
        Value = value;
    }

    public DateTime? Value { get; set; }

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

    /// <summary>
    /// Works only for vc:date-input.
    /// </summary>
    private static class DatePartFieldNamesProvider
    {
        public static (string Day, string Month, string Year) Get(string containerFieldName)
            => (containerFieldName + ".Day", containerFieldName + ".Month", containerFieldName + ".Year");
    }
}
