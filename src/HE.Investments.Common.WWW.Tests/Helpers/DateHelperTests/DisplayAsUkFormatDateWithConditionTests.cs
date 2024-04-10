using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class DisplayAsUkFormatDateWithConditionTests
{
    [Fact]
    public void ShouldReturnFormattedDateWithDescription_WhenAdditionalInputIsYes()
    {
        // given
        var date = new DateDetails("1", "1", "2022");
        const string additionalInput = "Yes";

        // when
        var result = DateHelper.DisplayAsUkFormatDateWithCondition(date, additionalInput);

        // then
        result.Should().Be("Yes, 01/01/2022");
    }

    [Fact]
    public void ShouldReturnNo_WhenAdditionalInputIsNo()
    {
        // given
        var date = new DateDetails("1", "1", "2022");
        const string additionalInput = "No";

        // when
        var result = DateHelper.DisplayAsUkFormatDateWithCondition(date, additionalInput);

        // then
        result.Should().Be("No");
    }
}
