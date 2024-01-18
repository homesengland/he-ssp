using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Programme;

public class AhpProgramme
{
    public AhpProgramme(DateOnly startAt, DateOnly endAt, MilestoneFramework milestoneFramework)
    {
        StartAt = startAt;
        EndAt = endAt;
        MilestoneFramework = milestoneFramework;
    }

    public DateOnly StartAt { get; }

    public DateOnly EndAt { get; }

    public MilestoneFramework MilestoneFramework { get; }
}
