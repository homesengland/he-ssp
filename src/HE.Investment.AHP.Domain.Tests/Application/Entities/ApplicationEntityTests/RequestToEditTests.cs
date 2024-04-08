using FluentAssertions;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class RequestToEditTests
{
    [Fact]
    public void ShouldThrowException_WhenUserIsNotPermittedToEdit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: false, canSubmitApplication: true)
            .Build();
        var reason = new RequestToEditReason("request to edit");

        // when
        var requestToEdit = () => testCandidate.RequestToEdit(reason);

        // then
        requestToEdit.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldThrowException_WhenStatusIsNotValidForRequestToEdit()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new RequestToEditReason("request to edit");

        // when
        var requestToEdit = () => testCandidate.RequestToEdit(reason);

        // then
        requestToEdit.Should().Throw<DomainException>();
    }

    [Fact]
    public void ShouldChangeStatusToRequestedEditing_WhenUserIsPermittedAndTransitionIsAllowed()
    {
        // given
        var testCandidate = new ApplicationEntityBuilder()
            .WithApplicationStatus(ApplicationStatus.ApplicationSubmitted)
            .WithUserPermissions(canEditApplication: true, canSubmitApplication: false)
            .Build();
        var reason = new RequestToEditReason("request to edit");

        // when
        testCandidate.RequestToEdit(reason);

        // then
        testCandidate.Status.Should().Be(ApplicationStatus.RequestedEditing);
        testCandidate.ChangeStatusReason.Should().Be("request to edit");
        testCandidate.IsStatusModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new ApplicationHasBeenRequestedToEditEvent(testCandidate.Id));
    }
}
