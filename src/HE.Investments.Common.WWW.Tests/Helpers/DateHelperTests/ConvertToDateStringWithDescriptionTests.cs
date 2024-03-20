using FluentAssertions;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.DateHelperTests;

public class ConvertToDateStringWithDescriptionTests
{
    [Fact]
    public void GivenAdditionalInputYes_WhenConvertToDateStringWithDescription_ThenReturnFormattedDateWithDescription()
    {
        // given
        const string day = "1";
        const string month = "1";
        const string year = "2022";
        const string additionalInput = "Yes";

        // when
        var result = DateHelper.ConvertToDateStringWithDescription(day, month, year, additionalInput);

        // then
        result.Should().Be("Yes, 01/01/2022");
    }

    [Fact]
    public void GivenAdditionalInputNo_WhenConvertToDateStringWithDescription_ThenReturnNo()
    {
        // given
        const string day = "1";
        const string month = "1";
        const string year = "2022";
        const string additionalInput = "No";

        // when
        var result = DateHelper.ConvertToDateStringWithDescription(day, month, year, additionalInput);

        // then
        result.Should().Be("No");
    }
}
