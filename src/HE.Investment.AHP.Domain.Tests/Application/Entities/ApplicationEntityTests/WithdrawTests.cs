using FluentAssertions;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class WithdrawTests
{
    [Fact]
    public void ShouldThrowException_WhenUserIsNotPermittedToEdit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: false, canSubmitApplication: true)
            .Build();
        var reason = new WithdrawReason("withdraw");

        // when
        var withdraw = () => testCandidate.Withdraw(reason);

        // then
        withdraw.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldChangeStatusToWithdrawn_WhenUserIsPermitted()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new WithdrawReason("withdraw");

        // when
        testCandidate.Withdraw(reason);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.Withdrawn);
        testCandidate.ChangeStatusReason.Should().Be("withdraw");
        testCandidate.IsStatusModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new ApplicationHasBeenWithdrawnEvent(testCandidate.Id));
    }

    [Fact]
    public void ShouldChangeStatusToWithdrawn_WhenApplicationHasDraftStatusButWasSubmitted()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder().WithWasSubmitted(true)
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new WithdrawReason("withdraw");

        // when
        testCandidate.Withdraw(reason);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.Withdrawn);
        testCandidate.ChangeStatusReason.Should().Be("withdraw");
        testCandidate.IsStatusModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new ApplicationHasBeenWithdrawnEvent(testCandidate.Id));
    }

    [Fact]
    public void ShouldChangeStatusToDeleted_WhenApplicationHasDraftStatusAndWasNotSubmitted()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new WithdrawReason("delete");

        // when
        testCandidate.Withdraw(reason);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.Deleted);
        testCandidate.ChangeStatusReason.Should().Be("delete");
        testCandidate.IsStatusModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new ApplicationHasBeenWithdrawnEvent(testCandidate.Id));
    }
}
