namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestonePaymentDate : DateValueObject
{
    public MilestonePaymentDate(string? day, string? month, string? year)
        : base(day, month, year, "ClaimMilestonePaymentAt", "milestone payment date")
    {
    }

    private MilestonePaymentDate(DateOnly value)
        : base(value)
    {
    }

    public static MilestonePaymentDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new MilestonePaymentDate(day, month, year) : null;
    }

    public static MilestonePaymentDate Create(DateOnly value)
    {
        return new MilestonePaymentDate(value);
    }
}
