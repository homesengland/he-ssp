using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.EventHandlers;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.EventHandlers;

public class SendUserInvitedNotificationEventHandlerTests : TestBase<SendUserInvitedNotificationEventHandler>
{
    [Fact]
    public async Task ShouldPublishUserInvitedNotification()
    {
        // given
        var domainEvent = new UserInvitedEvent("John", "Paul");
        var notificationPublisher = CreateAndRegisterDependencyMock<INotificationPublisher>();

        // when
        await TestCandidate.Handle(domainEvent, CancellationToken.None);

        // then
        notificationPublisher.Verify(
            x => x.Publish(It.Is<UserInvitedNotification>(y => y.TemplateText("<UserFullName> invited") == "John Paul invited")),
            Times.Once);
    }
}
