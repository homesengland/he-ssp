using FluentAssertions;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Loans.Common.Tests.Extensions.CommonAnswersMappingExtensions;

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
