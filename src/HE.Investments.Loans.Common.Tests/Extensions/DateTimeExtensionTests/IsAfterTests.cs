using FluentAssertions;
using HE.Investments.Loans.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HE.Investments.Loans.Common.Tests.Extensions.DateTimeExtensionTests;

[TestClass]
public class IsAfterTests
{
    [TestMethod]
    public void ReturnFalseWhenProvidedDateIsBeforeOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5);
        var otherDate = date.AddSeconds(1);

        // given & then
        date.IsAfter(otherDate).Should().BeFalse();
    }

    [TestMethod]
    public void ReturnFalseWhenProvidedDateIsEqualToOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5);
        var otherDate = new DateTime(2023, 7, 5);

        // given & then
        date.IsAfter(otherDate).Should().BeFalse();
    }

    [TestMethod]
    public void ReturnTrueWhenProvidedDateIsAfterOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5);
        var otherDate = date.AddSeconds(-1);

        // given & then
        date.IsAfter(otherDate).Should().BeTrue();
    }
}
