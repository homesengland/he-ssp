namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteDate : DateValueObject
{
    public StartOnSiteDate(string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", "start on site date")
    {
    }

    private StartOnSiteDate(DateOnly value)
        : base(value)
    {
    }

    public static StartOnSiteDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new StartOnSiteDate(day, month, year) : null;
    }

    public static StartOnSiteDate Create(DateOnly value)
    {
        return new StartOnSiteDate(value);
    }
}
