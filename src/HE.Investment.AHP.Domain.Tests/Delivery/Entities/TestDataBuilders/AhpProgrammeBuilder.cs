using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class AhpProgrammeBuilder
{
    public AhpProgramme Build()
    {
        return new(
            "1",
            "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
            new ProgrammeDates(
                new DateOnly(2023, 1, 1),
                new DateOnly(2027, 1, 1)),
            MilestoneFramework.Default);
    }
}
