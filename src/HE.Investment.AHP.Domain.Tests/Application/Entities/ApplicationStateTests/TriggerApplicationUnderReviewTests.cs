using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationStateTests;

public class TriggerApplicationUnderReviewTests : TriggerTestBase
{
    [Theory]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.Withdraw, ApplicationStatus.Withdrawn)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.PutOnHold, ApplicationStatus.OnHold)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.RequestToEdit, ApplicationStatus.RequestedEditing)]
    public void ShouldAllowTransition_WhenOperationIsPossibleAndUserIsPermitted(ApplicationStatus applicationStatus, AhpApplicationOperation operation, ApplicationStatus expectedStatus)
    {
        // given
        var testCandidate = BuildApplicationState(applicationStatus, canEditApplication: true);

        // when
        var result = testCandidate.Trigger(operation);

        // then
        result.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.Modification)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.Modification)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.Modification)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.Submit)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.Submit)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.Submit)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.Reactivate)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.Reactivate)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.Reactivate)]
    public void ShouldThrowException_WhenOperationIsNotPossibleAndUserIsPermitted(ApplicationStatus applicationStatus, AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            applicationStatus,
            canEditApplication: true,
            canSubmitApplication: true,
            previousApplicationStatus: ApplicationStatus.Draft);

        // when
        var trigger = () => testCandidate.Trigger(operation);

        // then
        trigger.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.Withdraw)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.Withdraw)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.Withdraw)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.PutOnHold)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.PutOnHold)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.PutOnHold)]
    [InlineData(ApplicationStatus.UnderReview, AhpApplicationOperation.RequestToEdit)]
    [InlineData(ApplicationStatus.ApplicationUnderReview, AhpApplicationOperation.RequestToEdit)]
    [InlineData(ApplicationStatus.CashflowUnderReview, AhpApplicationOperation.RequestToEdit)]
    public void ShouldThrowException_WhenOperationPossibleButUserIsNotPermitted(ApplicationStatus applicationStatus, AhpApplicationOperation operation)
    {
        // given
        var testCandidate = BuildApplicationState(
            applicationStatus,
            canEditApplication: false,
            canSubmitApplication: false);

        // when
        var trigger = () => testCandidate.Trigger(operation);

        // then
        trigger.Should().Throw<DomainException>();
    }
}
