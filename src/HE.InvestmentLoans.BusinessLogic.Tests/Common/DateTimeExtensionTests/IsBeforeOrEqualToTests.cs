using FluentAssertions;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Common.DateTimeExtensionTests
{
    [TestClass]
    public class IsBeforeOrEqualToTests
    {
        [TestMethod]
        public void Return_true_when_provided_date_is_before_other_date()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = date.AddSeconds(1);

            date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
        }

        [TestMethod]
        public void Return_true_when_provided_date_is_equal_to_other_date()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = new DateTime(2023, 7, 5);

            date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
        }

        [TestMethod]
        public void Return_false_when_provided_date_is_after_other_date()
        {
            var date = new DateTime(2023, 7, 5);
            var otherDate = date.AddSeconds(-1);

            date.IsBeforeOrEqualTo(otherDate).Should().BeFalse();
        }
    }
}
