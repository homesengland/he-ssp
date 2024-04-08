using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationStateTests;

public class TriggerApplicationSubmittedTests : TriggerTestBase
{
    [Theory]
    [InlineData(AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn)]
    [InlineData(AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold)]
    [InlineData(AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing)]
    public void ShouldAllowTransition_WhenOperationIsPossibleAndUserIsPermitted(AhpApplicationOperation operation, ApplicationStatus expectedStatus)
    {
        // given
        var testCandidate = BuildApplicationState(ApplicationStatus.ApplicationSubmitted, canEditApplication: true);

        // when
        var result = testCandidate.Trigger(operation);

        // then
        result.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(AhpApplicationOperation.Modification)]
    [InlineData(AhpApplicationOperation.Submit)]
    [InlineData(AhpApplicationOperation.Reactivate)]
    public void ShouldThrowException_WhenOperationIsNotPossibleAndUserIsPermitted(AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            ApplicationStatus.ApplicationSubmitted,
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
    [InlineData(AhpApplicationOperation.PutOnHold)]
    [InlineData(AhpApplicationOperation.RequestToEdit)]
    public void ShouldThrowException_WhenOperationPossibleButUserIsNotPermitted(AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            ApplicationStatus.ApplicationSubmitted,
            canEditApplication: false,
            canSubmitApplication: false);

        // when
        var trigger = () => testCandidate.Trigger(operation);

        // then
        trigger.Should().Throw<DomainException>();
    }
}
