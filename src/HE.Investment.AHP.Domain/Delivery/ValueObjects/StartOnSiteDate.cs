using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class StartOnSiteDate : DateValueObject
{
    private const string FieldDescription = "start on site date";

    public StartOnSiteDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists)
    {
    }

    public StartOnSiteDate(DateTime? value)
        : base(value)
    {
    }

    public static StartOnSiteDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
