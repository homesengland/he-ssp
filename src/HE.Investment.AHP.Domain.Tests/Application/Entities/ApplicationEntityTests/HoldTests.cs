using FluentAssertions;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class HoldTests
{
    [Fact]
    public void ShouldThrowException_WhenUserIsNotPermittedToEdit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: false, canSubmitApplication: true)
            .Build();
        var reason = new HoldReason("hold reason");

        // when
        var hold = () => testCandidate.Hold(reason);

        // then
        hold.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldThrowException_WhenStatusIsNotValidForHold()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.OnHold)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: true)
            .Build();
        var reason = new HoldReason("hold reason");

        // when
        var hold = () => testCandidate.Hold(reason);

        // then
        hold.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldChangeStatusToOnHold_WhenUserIsPermittedAndTransitionIsAllowed()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new HoldReason("hold reason");

        // when
        testCandidate.Hold(reason);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.OnHold);
        testCandidate.ChangeStatusReason.Should().Be("hold reason");
        testCandidate.IsStatusModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new ApplicationHasBeenPutOnHoldEvent(testCandidate.Id));
    }
}
