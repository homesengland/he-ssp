using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestData;
using HE.Investments.FrontDoor.Domain.Tests.Programme.TestObjectBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Programme.ProgrammeRepositoryTests;

public class GetProgrammesTests
{
    [Fact]
    public async Task ShouldReturnProgrammes()
    {
        // given
        var programmeRepository = ProgrammeRepositoryTestBuilder
            .New()
            .ReturnProgrammes()
            .Build();

        // when
        var result = await programmeRepository.GetProgrammes(CancellationToken.None);
        var programmeDetailsList = result.ToList();

        // then
        programmeDetailsList.Should().HaveCount(1);
        programmeDetailsList.Should().Contain(ProgrammeTestData.ImportantProgramme);
    }
}
