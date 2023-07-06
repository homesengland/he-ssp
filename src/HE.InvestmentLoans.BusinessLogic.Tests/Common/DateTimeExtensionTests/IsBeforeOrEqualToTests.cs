using FluentAssertions;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Common.DateTimeExtensionTests
{
    [TestClass]
    public class IsBeforeOrEqualToTests
    {
        [TestMethod]
        public void ReturnTrueWhenProvidedDateIsBeforeOtherDate()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = date.AddSeconds(1);

            date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
        }

        [TestMethod]
        public void ReturnTrueWhenProvidedDateIsEqualToOtherDate()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = new DateTime(2023, 7, 5);

            date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
        }

        [TestMethod]
        public void ReturnFalseWhenProvidedDateIsAfterOtherDate()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = date.AddSeconds(-1);

            date.IsBeforeOrEqualTo(otherDate).Should().BeFalse();
        }
    }
}
