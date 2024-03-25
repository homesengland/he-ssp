using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;

public class PurchaseDateTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldThrowException_WhenAllDatePartsAreMissing()
    {
        // given & when
        var action = () => new PurchaseDate(null, null, null, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("Enter when you purchased this site");
    }

    [Theory]
    [InlineData("", "9", "2023", "The date when you purchased this site must include a day")]
    [InlineData("", "", "2023", "The date when you purchased this site must include a day and month")]
    [InlineData("", "9", "", "The date when you purchased this site must include a day and year")]
    [InlineData("24", "", "2023", "The date when you purchased this site must include a month")]
    [InlineData("24", "", "", "The date when you purchased this site must include a month and year")]
    [InlineData("24", "9", "", "The date when you purchased this site must include a year")]
    public void ShouldThrowException_WhenSomeDateIsMissingSomePart(string day, string month, string year, string expectedErrorMessage)
    {
        // given & when
        var action = () => new PurchaseDate(day, month, year, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData("32", "1", "2023")]
    [InlineData("1", "13", "2023")]
    [InlineData("1", "1", "-1")]
    public void ShouldThrowException_WhenDateIsNotReal(string day, string month, string year)
    {
        // given & when
        var action = () => new PurchaseDate(day, month, year, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("When you purchased this site must be a real date");
    }

    [Fact]
    public void ShouldThrowException_WhenFutureDateIsProvided()
    {
        // given
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now).Returns(new DateTime(2023, 10, 3, 12, 25, 0, DateTimeKind.Unspecified));

        // when
        var action = () => new PurchaseDate("4", "10", "2023", _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("The date you purchased this land must be today or in the past");
    }

    [Fact]
    public void ShouldCreateDate_WhenPastDateIsProvided()
    {
        // given
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now).Returns(new DateTime(2023, 10, 3, 12, 25, 0, DateTimeKind.Unspecified));

        // when
        var date = new PurchaseDate("2", "10", "2023", _dateTimeProvider);

        // then
        date.Value.Should().Be(new DateTime(2023, 10, 2, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void ShouldCreateDate_WhenPresentDateIsProvided()
    {
        // given
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now).Returns(new DateTime(2023, 10, 3, 12, 25, 0, DateTimeKind.Unspecified));

        // when
        var date = new PurchaseDate("3", "10", "2023", _dateTimeProvider);

        // then
        date.Value.Should().Be(new DateTime(2023, 10, 3, 0, 0, 0, DateTimeKind.Unspecified));
    }
}
