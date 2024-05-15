using HE.Investments.FrontDoor.Domain.Programme;

namespace HE.Investments.FrontDoor.Domain.Tests.Programme.TestData;

public static class ProgrammeTestData
{
    public static ProgrammeDetails ImportantProgramme => new(
        "Programme 1",
        new DateOnly(2024, 1, 1),
        new DateOnly(2026, 12, 31),
        new DateOnly(2025, 5, 5));
}
