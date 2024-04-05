using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationStateTests;

public class TriggerApplicationOnHoldTests : TriggerTestBase
{
    [Theory]
    [InlineData(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn)]
    [InlineData(AhpApplicationOperation.Reactivate, ApplicationStatus.Draft)]
    public void ShouldAllowTransition_WhenOperationIsPossibleAndUserIsPermitted(AhpApplicationOperation operation, ApplicationStatus expectedStatus)
    {
        // given
        var testCandidate = BuildApplicationState(
            ApplicationStatus.OnHold,
            canEditApplication: true,
            previousApplicationStatus: ApplicationStatus.Draft);

        // when
        var result = testCandidate.Trigger(operation);

        // then
        result.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(AhpApplicationOperation.Modification)]
    [InlineData(AhpApplicationOperation.PutOnHold)]
    [InlineData(AhpApplicationOperation.Submit)]
    [InlineData(AhpApplicationOperation.RequestToEdit)]
    public void ShouldThrowException_WhenOperationIsNotPossibleAndUserIsPermitted(AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            ApplicationStatus.OnHold,
            canEditApplication: true,
            canSubmitApplication: true,
            previousApplicationStatus: ApplicationStatus.Draft);

        // when
        var trigger = () => testCandidate.Trigger(operation);

        // then
        trigger.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(AhpApplicationOperation.Withdraw)]
    [InlineData(AhpApplicationOperation.Reactivate)]
    public void ShouldThrowException_WhenOperationPossibleButUserIsNotPermitted(AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            ApplicationStatus.OnHold,
            canEditApplication: false,
            canSubmitApplication: false,
            previousApplicationStatus: ApplicationStatus.Draft);

        // when
        var trigger = () => testCandidate.Trigger(operation);

        // then
        trigger.Should().Throw<DomainException>();
    }
}
