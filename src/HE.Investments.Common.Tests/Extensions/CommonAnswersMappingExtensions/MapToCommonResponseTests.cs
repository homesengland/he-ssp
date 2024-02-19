using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Common.Tests.Extensions.CommonAnswersMappingExtensions;

[TestClass]
public class MapToCommonResponseTests
{
    [TestMethod]
    public void MapTrueToYes()
    {
        bool? boolField = true;
        boolField.MapToCommonResponse().Should().Be(CommonResponse.Yes);
    }

    [TestMethod]
    public void MapFalseToNo()
    {
        bool? boolField = false;
        boolField.MapToCommonResponse().Should().Be(CommonResponse.No);
    }

    [TestMethod]
    public void MapNullToNull()
    {
        bool? boolField = null;
        boolField.MapToCommonResponse().Should().BeNull();
    }
}
