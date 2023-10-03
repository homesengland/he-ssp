using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
public class PurchaseDateTests
{
    private readonly DateTime _now = new(2023, 10, 3);

    [Fact]
    public void ShouldFail_WhenFutureDateIsProvided()
    {
        var action = () => PurchaseDate.FromString("2023", "10", "4", _now);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.FuturePurchaseDate);
    }

    [Fact]
    public void ShouldCreateDate_WhenPastDateIsProvided()
    {
        var date = PurchaseDate.FromString("2023", "10", "2", _now);

        date.AsDateTime().Should().Be(new DateTime(2023, 10, 2));
    }

    [Fact]
    public void ShouldCreateDate_WhenPresentDateIsProvided()
    {
        var date = PurchaseDate.FromString("2023", "10", "3", _now);

        date.AsDateTime().Should().Be(new DateTime(2023, 10, 3));
    }
}
