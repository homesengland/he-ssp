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
    public void ShouldFail_WhenStartDateExistButDateIsNotProvided()
    {
        var action = () => new StartDate(true, null);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoStartDate);
    }

    [Theory]
    [InlineData("", "9", "2023")]
    [InlineData("24", "", "2023")]
    [InlineData("24", "9", "")]
    public void ShouldFail_WhenStartDateExistButDateIsNotCompleted(string day, string month, string year)
    {
        var action = () => StartDate.From(CommonResponse.Yes, year, month, day);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoStartDate);
    }

    [Theory]
    [InlineData("32", "1", "2023")]
    [InlineData("1", "13", "2023")]
    [InlineData("1", "1", "-1")]
    [InlineData("31", "12", "1752")]
    public void ShouldFail_WhenStartDateExistButIsNotCorrect(string day, string month, string year)
    {
        var action = () => StartDate.From(CommonResponse.Yes, year, month, day);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.InvalidStartDate);
    }

    [Fact]
    public void ShouldCreateNonExistingStartDate_NoMatterWhatIsPassedAsDayMonthOrYear()
    {
        var startDate = StartDate.From(CommonResponse.No, "any", "any", "any");

        startDate.Exists.Should().BeFalse();
    }

    [Fact]
    public void ShouldCreateExistingStartDate_WhenCorrectDataIsProvided()
    {
        var startDate = StartDate.From(CommonResponse.Yes, "2023", "9", "24");

        startDate.Exists.Should().BeTrue();
        startDate.Value.Should().Be(new DateTime(2023, 9, 24, 0, 0, 0, DateTimeKind.Unspecified));
    }
}
