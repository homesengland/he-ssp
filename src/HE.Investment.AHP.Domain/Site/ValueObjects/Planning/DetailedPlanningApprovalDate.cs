using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class DetailedPlanningApprovalDate : DateValueObject
{
    private const string FieldDescription = "detailed planning approval was granted date";

    public DetailedPlanningApprovalDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(DetailedPlanningApprovalDate), FieldDescription, !exists)
    {
    }

    public DetailedPlanningApprovalDate(DateTime? value)
        : base(value)
    {
    }

    public static DetailedPlanningApprovalDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
