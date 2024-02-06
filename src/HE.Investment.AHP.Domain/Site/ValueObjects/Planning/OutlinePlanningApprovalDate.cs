using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class OutlinePlanningApprovalDate : DateValueObject
{
    public OutlinePlanningApprovalDate(string? day, string? month, string? year)
        : base(day, month, year, "OutlinePlanningApprovalDate", "outline planning approval date")
    {
    }

    public static OutlinePlanningApprovalDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new OutlinePlanningApprovalDate(day, month, year) : null;
    }
}
