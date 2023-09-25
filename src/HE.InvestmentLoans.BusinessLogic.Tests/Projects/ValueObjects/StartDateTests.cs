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
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
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
        var action = () => StartDate.From(CommonResponse.Yes, day, month, year);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoStartDate);
    }

    [Theory]
    [InlineData("32", "1", "2023")]
    [InlineData("1", "13", "2023")]
    [InlineData("1", "1", "-1")]
    public void ShouldFail_WhenStartDateExistButIsNotCorrect(string day, string month, string year)
    {
        var action = () => StartDate.From(CommonResponse.Yes, day, month, year);

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
        var startDate = StartDate.From(CommonResponse.Yes, "24", "9", "2023");

        startDate.Exists.Should().BeTrue();
        startDate.Value.Should().Be(new DateTime(2023, 9, 24));
    }
}
