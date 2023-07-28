using FluentAssertions;
using HE.InvestmentLoans.Common.Utils.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.InvestmentLoans.Common.Tests.ValueObjects;

[TestClass]
public class ControllerNameTests
{
    [TestMethod]
    public void ThrowWhenProvidedNameIsEmpty()
    {
        Action action = () => new ControllerName("");

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void ThrowWhenProvidedNameIsNull()
    {
        Action action = () => new ControllerName(null);

        action.Should().Throw<ArgumentException>();
    }


    [TestMethod]
    public void ReturnNameWithoutControllerPrefix()
    {
        var controllerName = new ControllerName("TmpController");

        controllerName.WithoutPrefix().Should().Be("Tmp");
    }
}
