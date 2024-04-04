using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ExpectedStartDate : ValueObject, IQuestion
{
    public ExpectedStartDate(string? month, string? year)
    {
        if (string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
        {
            ThrowValidationError("Enter when you expect to start the project");
        }

        if (string.IsNullOrWhiteSpace(year))
        {
            ThrowValidationError("The date when you expect to start the project must include a year", "Year");
        }

        if (string.IsNullOrWhiteSpace(month))
        {
            ThrowValidationError("The date when you expect to start the project must include a month", "Month");
        }

        Value = CreateDateOnly(month, year);
        if (Value == null)
        {
            ThrowValidationError("When you expect to start the project must be a real date");
        }
    }

    private ExpectedStartDate(int month, int year)
    {
        Value = new DateOnly(year, month, 1);
    }

    private ExpectedStartDate()
    {
    }

    public static ExpectedStartDate Empty => new();

    public DateOnly? Value { get; set; }

    public static ExpectedStartDate? Create(int? month, int? year) =>
        month.IsProvided() && year.IsProvided() ? new ExpectedStartDate(month!.Value, year!.Value) : null;

    public bool IsAnswered()
    {
        return Value.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private static DateOnly? CreateDateOnly(string? month, string? year)
    {
        if (int.TryParse(month, out var intMonth)
            && int.TryParse(year, out var intYear)
            && intYear is >= 1900 and <= 9999)
        {
            try
            {
                return new DateOnly(intYear, intMonth, 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        return null;
    }

    private static void ThrowValidationError(string message, string? datePart = null)
    {
        OperationResult.ThrowValidationError(
            datePart == null ? nameof(ExpectedStartDate) : $"{nameof(ExpectedStartDate)}.{datePart}",
            message);
    }
}
