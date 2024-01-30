using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

public class AhpProgrammeBuilder
{
    public AhpProgramme Build()
    {
        return new(
            new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            MilestoneFramework.Default);
    }
}
