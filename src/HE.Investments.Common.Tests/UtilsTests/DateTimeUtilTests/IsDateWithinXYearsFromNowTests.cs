using FluentAssertions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Common.Utils;
using Xunit;

namespace HE.Investments.Common.Tests.UtilsTests.DateTimeUtilTests;

public class IsDateWithinXYearsFromNowTests
{
    [Fact]
    public void ShouldReturnTrue_WhenDateIsWithinXYearsFromNow()
    {
        // given
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnCurrentDate().Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var date = "01.01.2020";
        var yearsToCheck = 4;

        // when
        var result = DateTimeUtil.IsDateWithinXYearsFromNow(date, yearsToCheck);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenDateIsNotWithinXYearsFromNow()
    {
        // given
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnCurrentDate().Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var date = "01.01.2019";
        var yearsToCheck = 2;

        // when
        var result = DateTimeUtil.IsDateWithinXYearsFromNow(date, yearsToCheck);

        // then
        result.Should().BeFalse();
    }
}
