using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.WWW.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.CommonAnswersMappingExtensions;

public class MapToCommonResponseTests
{
    [Fact]
    public void MapTrueToYes()
    {
        bool? boolField = true;
        boolField.MapToCommonResponse().Should().Be(CommonResponse.Yes);
    }

    [Fact]
    public void MapFalseToNo()
    {
        bool? boolField = false;
        boolField.MapToCommonResponse().Should().Be(CommonResponse.No);
    }

    [Fact]
    public void MapNullToNull()
    {
        bool? boolField = null;
        boolField.MapToCommonResponse().Should().BeNull();
    }
}
