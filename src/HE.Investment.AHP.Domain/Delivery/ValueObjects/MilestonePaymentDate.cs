using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestonePaymentDate : DateValueObject
{
    private const string FieldDescription = "milestone payment date";

    public MilestonePaymentDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "ClaimMilestonePaymentAt", FieldDescription, !exists)
    {
    }

    public MilestonePaymentDate(DateTime? value)
        : base(value)
    {
    }

    public static MilestonePaymentDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
