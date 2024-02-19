using FluentAssertions;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Common.Tests.Extensions.CommonAnswersMappingExtensions;

[TestClass]
public class MapToBoolTests
{
    [TestMethod]
    public void MapYesToTrue()
    {
        CommonResponse.Yes.MapToBool().Should().BeTrue();
    }

    [TestMethod]
    public void MapNoToFalse()
    {
        CommonResponse.No.MapToBool().Should().BeFalse();
    }

    [TestMethod]
    public void MapAnyOtherStringToNull()
    {
        "AnyString".MapToBool().Should().BeNull();
    }
}
