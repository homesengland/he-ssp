using System.Globalization;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public static class MilestonePaymentDateFactory
{
    public static MilestonePaymentDate GetDateDayAfter(MilestonePaymentDate? date)
    {
        if (date == null)
        {
            throw new InvalidOperationException("Cannot create new date one day after provided date");
        }

        var nextDate = date.Value!.Value.AddDays(1);

        return new MilestonePaymentDate(
            true,
            nextDate.Day.ToString(CultureInfo.InvariantCulture),
            nextDate.Month.ToString(CultureInfo.InvariantCulture),
            nextDate.Year.ToString(CultureInfo.InvariantCulture));
    }
}
