using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects.ExpectedStartDateTests;

public class ExpectedStartDateCtorTests
{
    public static IEnumerable<object[]> ValidDates => new[]
    {
        new object[] { "1", "1900", new DateOnly(1900, 1, 1) },
        new object[] { "03", "2024", new DateOnly(2024, 3, 1) },
        new object[] { "12", "9999", new DateOnly(9999, 12, 1) },
    };

    [Fact]
    public void ShouldThrowException_WhenYearAndMonthIsNotProvided()
    {
        // given & when
        var create = () => new ExpectedStartDate(null, null);

        // then
        var errors = create.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("ExpectedStartDate");
        errors[0].ErrorMessage.Should().Be("Enter when you expect to start the project");
    }

    [Fact]
    public void ShouldThrowException_WhenOnlyYearIsNotProvided()
    {
        // given & when
        var create = () => new ExpectedStartDate("10", null);

        // then
        var errors = create.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("ExpectedStartDate.Year");
        errors[0].ErrorMessage.Should().Be("The date when you expect to start the project must include a year");
    }

    [Fact]
    public void ShouldThrowException_WhenOnlyMonthIsNotProvided()
    {
        // given & when
        var create = () => new ExpectedStartDate(null, "2024");

        // then
        var errors = create.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("ExpectedStartDate.Month");
        errors[0].ErrorMessage.Should().Be("The date when you expect to start the project must include a month");
    }

    [Theory]
    [InlineData("January", "2024")]
    [InlineData("1", "Two thousand twenty four")]
    [InlineData("0", "2024")]
    [InlineData("13", "2024")]
    [InlineData("1", "1899")]
    [InlineData("1", "10000")]
    public void ShouldThrowException_WhenDateIsNotARealDate(string month, string year)
    {
        // given & when
        var create = () => new ExpectedStartDate(month, year);

        // then
        var errors = create.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors[0].AffectedField.Should().Be("ExpectedStartDate");
        errors[0].ErrorMessage.Should().Be("When you expect to start the project must be a real date");
    }

    [Theory]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "False positive returned by analyzer")]
    [MemberData(nameof(ValidDates))]
    public void ShouldCreateDate_WhenDateIsARealDate(string month, string year, DateOnly expectedDate)
    {
        // given & when
        var result = new ExpectedStartDate(month, year);

        // then
        result.Value.Should().NotBeNull();
        result.Value.Should().Be(expectedDate);
    }
}
