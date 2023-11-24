using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;
public class PurchaseDateTests
{
    private readonly DateTime _now = new(2023, 10, 3);

    [Theory]
    [InlineData("", "9", "2023")]
    [InlineData("24", "", "2023")]
    [InlineData("24", "9", "")]
    public void ShouldFail_WhenStartDateExistButDateIsNotCompleted(string day, string month, string year)
    {
        var action = () => PurchaseDate.FromString(year, month, day, _now);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoPurchaseDate);
    }

    [Theory]
    [InlineData("32", "1", "2023")]
    [InlineData("1", "13", "2023")]
    [InlineData("1", "1", "-1")]
    public void ShouldFail_WhenStartDateExistButIsNotCorrect(string day, string month, string year)
    {
        var action = () => PurchaseDate.FromString(year, month, day, _now);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.IncorrectPurchaseDate);
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
