using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ExpectedPlanningApprovalDate : DateValueObject
{
    private const string FieldDescription = "expected planning approval date";

    public ExpectedPlanningApprovalDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(ExpectedPlanningApprovalDate), FieldDescription, !exists)
    {
    }

    public ExpectedPlanningApprovalDate(DateTime? value)
        : base(value)
    {
    }

    public static ExpectedPlanningApprovalDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
