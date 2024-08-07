using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestData;

public static class ProgrammeTestData
{
    public static readonly Programme.Contract.Programme AhpCmeProgramme = new(
        new ProgrammeId("d5fe3baa-eeae-ee11-a569-0022480041cf"),
        "AHP 21-26 CME",
        "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
        true,
        ProgrammeType.Ahp,
        new DateRange(new DateOnly(2021, 01, 01), new DateOnly(2026, 01, 01)),
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
        new DateRange(new DateOnly(2024, 03, 01), new DateOnly(2025, 12, 01)),
        new DateRange(DateOnly.MinValue, DateOnly.MaxValue));
}
