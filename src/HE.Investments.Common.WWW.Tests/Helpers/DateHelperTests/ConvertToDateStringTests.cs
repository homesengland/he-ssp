using FluentAssertions;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class ConvertToDateStringTests
{
    [Fact]
    public void GivenDateDetails_WhenConvertToDateString_ThenReturnFormattedDate()
    {
        // given && when
        var result = DateHelper.ConvertToDateString("1", "1", "2022");

        // then
        result.Should().Be("01/01/2022");
    }

    [Fact]
    public void GivenInvalidDateDetails_WhenConvertToDateString_ThenReturnEmptyString()
    {
        // given && when
        var result = DateHelper.ConvertToDateString("1", "1", "invalid");

        // then
        result.Should().BeEmpty();
    }
}
