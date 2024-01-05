namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionDate : DateValueObject
{
    public CompletionDate(string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", "completion date")
    {
    }

    public static CompletionDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new CompletionDate(day, month, year) : null;
    }
}
