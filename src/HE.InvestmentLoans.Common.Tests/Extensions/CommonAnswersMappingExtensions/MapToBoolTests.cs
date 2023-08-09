using FluentAssertions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.InvestmentLoans.Common.Tests.Extensions.CommonAnswersMappingExtensions;

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
