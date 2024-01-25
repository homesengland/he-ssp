using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects;

public class AcquisitionMilestoneDetailsTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenPaymentDateBeforeAcquisitionDate()
    {
        // given && then
        var action = () => new AcquisitionMilestoneDetailsBuilder().WithPaymentDateBeforeMilestoneDate().Build();

        // then
        AssertException(action, "The milestone payment date must be on or after the milestone date");
    }

    [Fact]
    public void ShouldCreateAcquisitionMilestoneDetails()
    {
        // given && then
        var details = new AcquisitionMilestoneDetailsBuilder().WithPaymentDateAtMilestoneDate().Build();

        // then
        details.PaymentDate.Should().NotBeNull();
        details.PaymentDate!.Value.Should().Be(AcquisitionMilestoneDetailsBuilder.DefaultAcquisitionDate!.Value);
    }

    private static void AssertException(Func<AcquisitionMilestoneDetails> action, string errorMessage)
    {
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == errorMessage);
    }
}
