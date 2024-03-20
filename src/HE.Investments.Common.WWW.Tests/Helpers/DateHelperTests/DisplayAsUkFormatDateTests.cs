using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class DisplayAsUkFormatDateTests
{
    [Fact]
    public void GivenNullDateDetails_WhenDisplayAsUkFormatDate_ThenReturnNull()
    {
        // given
        DateDetails? date = null;

        // when
        var result = DateHelper.DisplayAsUkFormatDate(date);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void GivenDateDetails_WhenDisplayAsUkFormatDate_ThenReturnFormattedDate()
    {
        // given
        var date = new DateDetails("1", "1", "2022");

        // when
        var result = DateHelper.DisplayAsUkFormatDate(date);

        // then
        result.Should().Be("01/01/2022");
    }
}
