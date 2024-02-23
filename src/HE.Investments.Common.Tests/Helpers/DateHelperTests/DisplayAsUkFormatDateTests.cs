using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;
using Xunit;

namespace HE.Investments.Common.Tests.Helpers.DateHelperTests;

public class DisplayAsUkFormatDateTests
{
    [Theory]
    [InlineData(null, null, null)]
    [InlineData("10", "11", null)]
    [InlineData("20", "", "2020")]
    [InlineData("", "12", "2024")]
    public void ShouldReturnNull_WhenDateIsEmpty(string? day, string? month, string? year)
    {
        // given
        var date = new DateDetails(day, month, year);

        // when
        var result = DateHelper.DisplayAsUkFormatDate(date);

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("32", "12", "2024")]
    [InlineData("1", "13", "2024")]
    [InlineData("1", "1", "-2024")]
    public void ShouldReturnNull_WhenDateIsNotRealDate(string day, string month, string year)
    {
        // given
        var date = new DateDetails(day, month, year);

        // when
        var result = DateHelper.DisplayAsUkFormatDate(date);

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("31", "12", "2024", "31/12/2024")]
    [InlineData("01", "02", "2020", "01/02/2020")]
    [InlineData("1", "2", "2020", "01/02/2020")]
    public void ShouldReturnUkFormattedDate_WhenDateIsRealDate(string day, string month, string year, string expectedResult)
    {
        // given
        var date = new DateDetails(day, month, year);

        // when
        var result = DateHelper.DisplayAsUkFormatDate(date);

        // then
        result.Should().Be(expectedResult);
    }
}
