using FluentAssertions;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Loans.Common.Tests.Extensions.CommonAnswersMappingExtensions;

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
