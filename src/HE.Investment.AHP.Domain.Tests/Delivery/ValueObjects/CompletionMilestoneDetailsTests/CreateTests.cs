using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.CompletionMilestoneDetailsTests;

public class CreateTests
{
    [Theory]
    [InlineData("", "1", "2024", "day")]
    [InlineData("1", "", "2024", "month")]
    [InlineData("1", "1", "", "year")]
    [InlineData("1", "", "", "month and year")]
    [InlineData("", "1", "", "day and year")]
    [InlineData("", "", "2024", "day and month")]
    public void ShouldThrowDomainValidationException_WhenDateIsRequiredsAndSomePartsAreAbsent(string day, string month, string year, string missingPartsText)
    {
        // when
        var date = () => new CompletionDate(true, day, month, year);

        // then
        date.Should().Throw<DomainValidationException>().WithMessage($"The completion date must include a {missingPartsText}");
    }

    [Fact]
    public void ShouldCreateEmptyDate_WhenDateIsOptional()
    {
        // when
        var date = new CompletionDate(false, string.Empty, string.Empty, string.Empty);

        date.Should().NotBeNull();
        date.Value.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenPaymentDateBeforeCompletionDate()
    {
        // given
        var milestoneDate = new CompletionDate(true, "10", "01", "2024");
        var paymentDate = new MilestonePaymentDate(true, "09", "01", "2024");

        // when
        var create = () => CompletionMilestoneDetails.Create(milestoneDate, paymentDate);

        // then
        create.Should().Throw<DomainValidationException>().WithMessage("The completion date must be before, or the same as, the forecast completion claim date");
    }

    [Fact]
    public void ShouldReturnNull_WhenMilestoneAndPaymentDateIsNull()
    {
        // given & when
        var result = CompletionMilestoneDetails.Create(null, null);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldCreateCompletionMilestoneDetails_WhenOnlyMilestoneDateIsProvided()
    {
        // given
        var milestoneDate = new CompletionDate(true, "10", "01", "2024");

        // when
        var result = CompletionMilestoneDetails.Create(milestoneDate, null);

        // then
        result.Should().NotBeNull();
        result!.MilestoneDate.Should().Be(milestoneDate);
        result.PaymentDate.Should().BeNull();
    }

    [Fact]
    public void ShouldCreateCompletionMilestoneDetails_WhenOnlyPaymentDateIsProvided()
    {
        // given
        var paymentDate = new MilestonePaymentDate(true, "10", "01", "2024");

        // when
        var result = CompletionMilestoneDetails.Create(null, paymentDate);

        // then
        result.Should().NotBeNull();
        result!.MilestoneDate.Should().BeNull();
        result.PaymentDate.Should().Be(paymentDate);
    }

    [Theory]
    [InlineData("10")]
    [InlineData("20")]
    public void ShouldCreateCompletionMilestoneDetails_WhenPaymentDateIsNotBeforeMilestoneDate(string paymentDay)
    {
        // given
        var milestoneDate = new CompletionDate(true, "10", "01", "2024");
        var paymentDate = new MilestonePaymentDate(true, paymentDay, "01", "2024");

        // when
        var result = CompletionMilestoneDetails.Create(milestoneDate, paymentDate);

        // then
        result.Should().NotBeNull();
        result!.MilestoneDate.Should().Be(milestoneDate);
        result.PaymentDate.Should().Be(paymentDate);
    }
}
