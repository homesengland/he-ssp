using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class AcquisitionDate : DateValueObject
{
    private const string FieldDescription = "acquisition date";

    public AcquisitionDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists)
    {
    }

    public AcquisitionDate(DateTime? value)
        : base(value)
    {
    }

    public static AcquisitionDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
