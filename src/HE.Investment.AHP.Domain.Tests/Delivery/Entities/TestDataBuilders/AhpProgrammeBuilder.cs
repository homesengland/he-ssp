using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class AhpProgrammeBuilder
{
    public AhpProgramme Build()
    {
        return new(
            new ProgrammeDates(
                new DateOnly(2023, 1, 1),
                new DateOnly(2027, 1, 1)),
            MilestoneFramework.Default);
    }
}
