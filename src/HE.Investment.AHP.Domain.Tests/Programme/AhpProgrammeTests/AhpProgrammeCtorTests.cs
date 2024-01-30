using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.TestsUtils.TestData;

namespace HE.Investment.AHP.Domain.Tests.Programme.AhpProgrammeTests;

public class AhpProgrammeCtorTests
{
    [Fact]
    public void ShouldThrowException_WhenStartAtIsNull()
    {
        // given & when
        var create = () => new AhpProgramme(null, DateTimeTestData.OctoberDay05Year2023At0858, MilestoneFramework.Default);

        // then
        create.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("startAt");
    }

    [Fact]
    public void ShouldThrowException_WhenEndAtIsNull()
    {
        // given & when
        var create = () => new AhpProgramme(DateTimeTestData.OctoberDay05Year2023At0858, null, MilestoneFramework.Default);

        // then
        create.Should().Throw<ArgumentException>().Which.ParamName.Should().Be("endAt");
    }

    [Fact]
    public void ShouldThrowException_WhenStartAtIsAfterEndAt()
    {
        // given & when
        var create = () => new AhpProgramme(
            DateTimeTestData.OctoberDay05Year2023At0858.AddDays(1),
            DateTimeTestData.OctoberDay05Year2023At0858,
            MilestoneFramework.Default);

        // then
        create.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldCreateEntity_WhenStartAtIsBeforeEndAt()
    {
        // given & when
        var result = new AhpProgramme(
            DateTimeTestData.OctoberDay05Year2023At0858,
            DateTimeTestData.OctoberDay05Year2023At0858.AddYears(1),
            MilestoneFramework.Default);

        // then
        result.StartAt.Should().Be(new DateOnly(2023, 10, 05));
        result.EndAt.Should().Be(new DateOnly(2024, 10, 05));
        result.MilestoneFramework.Should().Be(MilestoneFramework.Default);
    }
}
