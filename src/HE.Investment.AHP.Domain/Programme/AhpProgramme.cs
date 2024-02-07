using Dawn;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Programme;

public record AhpProgramme
{
    public AhpProgramme(DateTime? startAt, DateTime? endAt, MilestoneFramework milestoneFramework)
    {
        StartAt = DateOnly.FromDateTime(Guard.Argument(startAt, nameof(startAt)).NotNull().Value);
        EndAt = DateOnly.FromDateTime(Guard.Argument(endAt, nameof(endAt)).NotNull().Value);
        MilestoneFramework = milestoneFramework;

        if (StartAt > EndAt)
        {
            throw new DomainValidationException(nameof(StartAt), "AHP programme end date should be after start date.");
        }
    }

    public DateOnly StartAt { get; }

    public DateOnly EndAt { get; }

    public MilestoneFramework MilestoneFramework { get; }
}
