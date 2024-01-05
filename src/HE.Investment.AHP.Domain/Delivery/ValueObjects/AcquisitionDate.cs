namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class AcquisitionDate : DateValueObject
{
    public AcquisitionDate(string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", "acquisition date")
    {
    }

    public static AcquisitionDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new AcquisitionDate(day, month, year) : null;
    }
}
