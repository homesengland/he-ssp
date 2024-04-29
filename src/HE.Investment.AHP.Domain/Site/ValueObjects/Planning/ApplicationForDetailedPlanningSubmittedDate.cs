using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ApplicationForDetailedPlanningSubmittedDate : DateValueObject
{
    private const string FieldDescription = "application for detailed planning submitted date";

    public ApplicationForDetailedPlanningSubmittedDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(ApplicationForDetailedPlanningSubmittedDate), FieldDescription, !exists)
    {
    }

    public ApplicationForDetailedPlanningSubmittedDate(DateTime? value)
        : base(value)
    {
    }

    public static ApplicationForDetailedPlanningSubmittedDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
