using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class DetailedPlanningApprovalDate : DateValueObject
{
    public DetailedPlanningApprovalDate(string? day, string? month, string? year)
        : base(day, month, year, "DetailedPlanningApprovalDate", "detailed planning approval was granted date")
    {
    }

    public static DetailedPlanningApprovalDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new DetailedPlanningApprovalDate(day, month, year) : null;
    }
}
