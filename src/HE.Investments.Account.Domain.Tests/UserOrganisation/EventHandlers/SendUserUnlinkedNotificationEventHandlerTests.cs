using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Domain.UserOrganisation.EventHandlers;
using HE.Investments.Account.Domain.UserOrganisation.Notifications;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.EventHandlers;

public class SendUserUnlinkedNotificationEventHandlerTests : TestBase<SendUserUnlinkedNotificationEventHandler>
{
    [Fact]
    public async Task ShouldPublishUserUnlinkedNotification()
    {
        // given
        var domainEvent = new UserUnlinkedEvent(new UserGlobalId("global-id"), "John", "Paul");
        var notificationPublisher = CreateAndRegisterDependencyMock<INotificationPublisher>();

        // when
        await TestCandidate.Handle(domainEvent, CancellationToken.None);

        // then
        notificationPublisher.Verify(
            x => x.Publish(It.Is<UserUnlinkedNotification>(y => y.TemplateText("<UserFullName> unlinked") == "John Paul unlinked")),
            Times.Once);
    }
}
