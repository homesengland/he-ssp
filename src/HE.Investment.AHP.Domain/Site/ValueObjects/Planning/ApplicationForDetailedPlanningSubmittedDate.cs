using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ApplicationForDetailedPlanningSubmittedDate : DateValueObject
{
    public ApplicationForDetailedPlanningSubmittedDate(string? day, string? month, string? year)
        : base(day, month, year, "ApplicationForDetailedPlanningSubmittedDate", "application for detailed planning submitted date")
    {
    }

    public static ApplicationForDetailedPlanningSubmittedDate? Create(string? day, string? month, string? year)
    {
        return ValuesProvided(day, month, year) ? new ApplicationForDetailedPlanningSubmittedDate(day, month, year) : null;
    }
}
