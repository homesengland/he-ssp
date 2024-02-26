using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.WWW.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.CommonAnswersMappingExtensions;

public class MapToBoolTests
{
    [Fact]
    public void MapYesToTrue()
    {
        CommonResponse.Yes.MapToBool().Should().BeTrue();
    }

    [Fact]
    public void MapNoToFalse()
    {
        CommonResponse.No.MapToBool().Should().BeFalse();
    }

    [Fact]
    public void MapAnyOtherStringToNull()
    {
        "AnyString".MapToBool().Should().BeNull();
    }
}
