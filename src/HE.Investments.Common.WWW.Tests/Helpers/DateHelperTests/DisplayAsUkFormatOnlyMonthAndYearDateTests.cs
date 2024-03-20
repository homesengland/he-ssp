using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class DisplayAsUkFormatOnlyMonthAndYearDateTests
{
    [Fact]
    public void GivenNullDateDetails_WhenDisplayAsUkFormatOnlyMonthAndYearDate_ThenReturnNull()
    {
        // given
        DateDetails? date = null;

        // when
        var result = DateHelper.DisplayAsUkFormatOnlyMonthAndYearDate(date);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void GivenDateDetails_WhenDisplayAsUkFormatOnlyMonthAndYearDate_ThenReturnFormattedDate()
    {
        // given
        var date = new DateDetails("1", "1", "2022");

        // when
        var result = DateHelper.DisplayAsUkFormatOnlyMonthAndYearDate(date);

        // then
        result.Should().Be("01/2022");
    }
}
