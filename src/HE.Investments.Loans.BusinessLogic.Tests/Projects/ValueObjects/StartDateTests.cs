using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;

public class StartDateTests
{
    [Fact]
    public void ShouldFail_WhenStartDateExistButDateIsEmpty()
    {
        var action = () => new StartDate(true, string.Empty, string.Empty, string.Empty);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("Enter the project start date");
    }

    [Theory]
    [InlineData("", "9", "2023", "day")]
    [InlineData("", "", "2023", "day and month")]
    [InlineData("", "9", "", "day and year")]
    [InlineData("24", "", "2023", "month")]
    [InlineData("24", "", "", "month and year")]
    [InlineData("24", "9", "", "year")]
    public void ShouldFail_WhenStartDateExistButDateIsNotCompleted(string day, string month, string year, string expectedMessage)
    {
        var action = () => new StartDate(true, day, month, year);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage($"The project start date must include a {expectedMessage}");
    }

    [Theory]
    [InlineData("1", "1", "1899")]
    [InlineData("1", "1", "10000")]
    [InlineData("32", "1", "2023")]
    [InlineData("1", "13", "2023")]
    [InlineData("1", "1", "-1")]
    [InlineData("31", "12", "1752")]
    public void ShouldFail_WhenStartDateExistButIsNotCorrect(string day, string month, string year)
    {
        var action = () => new StartDate(true, day, month, year);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage("The project start date must be a real date");
    }

    [Fact]
    public void ShouldCreateNonExistingStartDate_NoMatterWhatIsPassedAsDayMonthOrYear()
    {
        var startDate = new StartDate(false, "any", "any", "any");

        startDate.Value.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ShouldCreateExistingStartDate_WhenCorrectDataIsProvided()
    {
        var startDate = new StartDate(true, "24", "9", "2023");

        startDate.Value.HasValue.Should().BeTrue();
        startDate.Value.Should().Be(new DateTime(2023, 9, 24, 0, 0, 0, DateTimeKind.Unspecified));
    }
}
