using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.CompletionMilestoneDetailsTests;

public class CreateTests
{
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
