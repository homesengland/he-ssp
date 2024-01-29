using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ExpectedPlanningApprovalDate : DateValueObject
{
    public ExpectedPlanningApprovalDate(string? day, string? month, string? year)
        : base(day, month, year, "ExpectedPlanningApprovalDate", "expected planning approval date")
    {
    }

    public static ExpectedPlanningApprovalDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new ExpectedPlanningApprovalDate(day, month, year) : null;
    }
}
