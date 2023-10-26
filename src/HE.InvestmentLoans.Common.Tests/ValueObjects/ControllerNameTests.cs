using FluentAssertions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.InvestmentLoans.Common.Tests.ValueObjects;

[TestClass]
public class ControllerNameTests
{
    [TestMethod]
    public void ThrowWhenProvidedNameIsEmpty()
    {
        Action action = () => _ = new ControllerName(string.Empty);

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void ThrowWhenProvidedNameIsNull()
    {
        Action action = () => _ = new ControllerName(null!);

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void ReturnNameWithoutControllerPrefix()
    {
        var controllerName = new ControllerName("TmpController");

        controllerName.WithoutPrefix().Should().Be("Tmp");
    }
}
