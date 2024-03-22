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
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var date = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var yearsToCheck = 3;

        // when
        var result = DateTimeUtil.IsDateWithinXYearsFromNow(date, yearsToCheck);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenDateIsNotWithinXYearsFromNow()
    {
        // given
        var dateTimeProviderMock = DateTimeProviderBuilder.New().ReturnDate(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Build();
        DateTimeUtil.SetDateTimeProvider(dateTimeProviderMock);
        var date = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var yearsToCheck = 1;

        // when
        var result = DateTimeUtil.IsDateWithinXYearsFromNow(date, yearsToCheck);

        // then
        result.Should().BeFalse();
    }
}
