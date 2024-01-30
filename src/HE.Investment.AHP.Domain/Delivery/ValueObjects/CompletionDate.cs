namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionDate : DateValueObject
{
    public CompletionDate(string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", "completion date")
    {
    }

    private CompletionDate(DateOnly value)
        : base(value)
    {
    }

    public static CompletionDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new CompletionDate(day, month, year) : null;
    }

    public static CompletionDate Create(DateOnly value)
    {
        return new CompletionDate(value);
    }
}
