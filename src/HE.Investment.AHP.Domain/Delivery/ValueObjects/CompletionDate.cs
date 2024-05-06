using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class CompletionDate : DateValueObject
{
    private const string FieldDescription = "completion date";

    public CompletionDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, "MilestoneStartAt", FieldDescription, !exists)
    {
    }

    public CompletionDate(DateTime? value)
        : base(value)
    {
    }

    public static CompletionDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
