using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class PlanningSubmissionDate : DateValueObject
{
    private const string FieldDescription = "planning submission date";

    public PlanningSubmissionDate(bool exists, string? day, string? month, string? year)
        : base(day, month, year, nameof(PlanningSubmissionDate), FieldDescription, !exists)
    {
    }

    public PlanningSubmissionDate(DateTime? value)
        : base(value)
    {
    }

    public static PlanningSubmissionDate FromDateDetails(bool exists, DateDetails? date) =>
        new(exists, date?.Day, date?.Month, date?.Year);
}
