using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ReactivateTests
{
    [Fact]
    public void ShouldThrowException_WhenUserIsNotPermittedToEdit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.OnHold)
            .WithPreviousStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: false, canSubmitApplication: true)
            .Build();

        // when
        var reactivate = () => testCandidate.Reactivate();

        // then
        reactivate.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldThrowException_WhenStatusIsNotValidForReactivate()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Withdrawn)
            .WithPreviousStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: true)
            .Build();

        // when
        var reactivate = () => testCandidate.Reactivate();

        // then
        reactivate.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldChangeStatusToPreviousStatus_WhenUserIsPermittedAndTransitionIsAllowed()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.OnHold)
            .WithPreviousStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();

        // when
        testCandidate.Reactivate();

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.ApplicationSubmitted);
        testCandidate.ChangeStatusReason.Should().BeNull();
        testCandidate.IsStatusModified.Should().BeTrue();
    }
}
