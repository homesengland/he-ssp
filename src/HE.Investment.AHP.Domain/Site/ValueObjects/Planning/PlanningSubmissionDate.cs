using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class PlanningSubmissionDate : DateValueObject
{
    public PlanningSubmissionDate(string? day, string? month, string? year)
        : base(day, month, year, "PlanningSubmissionDate", "planning submission date")
    {
    }

    public static PlanningSubmissionDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new PlanningSubmissionDate(day, month, year) : null;
    }
}
