namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteDate : DateValueObject
{
    public StartOnSiteDate(string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", "start on site date")
    {
    }

    public static StartOnSiteDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new StartOnSiteDate(day, month, year) : null;
    }
}
