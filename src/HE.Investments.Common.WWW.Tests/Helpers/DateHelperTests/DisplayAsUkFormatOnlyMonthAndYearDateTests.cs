using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class DisplayAsUkFormatOnlyMonthAndYearDateTests
{
    [Fact]
    public void ShouldReturnNull_WhenDateDetailsIsNull()
    {
        // given
        DateDetails? date = null;

        // when
        var result = DateHelper.DisplayAsUkFormatOnlyMonthAndYearDate(date);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnFormattedDate_WhenDateDetailsAreValid()
    {
        // given
        var date = new DateDetails("1", "1", "2022");

        // when
        var result = DateHelper.DisplayAsUkFormatOnlyMonthAndYearDate(date);

        // then
        result.Should().Be("01/2022");
    }
}
