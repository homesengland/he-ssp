using HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.EventHandlersTests;

public class LoanApplicationHasBeenResubmittedEventHandlerTests : TestBase<LoanApplicationHasBeenResubmittedEventHandler>
{
    [Fact]
    public async Task ShouldCallNotificationServiceWithNotifySuccessAction()
    {
        // given
        var notificationServiceMock = NotificationServiceMockTestBuilder.New().BuildMockAndRegister(this);

        // when
        await TestCandidate.Handle(new LoanApplicationHasBeenResubmittedEvent(), CancellationToken.None);

        // then
        notificationServiceMock.Verify(x => x.Publish(It.IsAny<LoanApplicationHasBeenResubmittedNotification>()), Times.Once);
    }
}
