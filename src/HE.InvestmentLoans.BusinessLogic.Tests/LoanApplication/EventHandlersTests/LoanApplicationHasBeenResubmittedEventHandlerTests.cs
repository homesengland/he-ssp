using HE.InvestmentLoans.BusinessLogic.LoanApplication.EventHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.EventHandlersTests;

public class LoanApplicationHasBeenResubmittedEventHandlerTests : TestBase<LoanApplicationHasBeenResubmittedEventHandler>
{
    [Fact]
    public async Task ShouldCallNotificationServiceWithNotifySuccessAction()
    {
        // given
        var notificationServiceMock = NotificationServiceMockTestBuilder.New().BuildMockAndRegister(this);

        var applicationResubmittedBodyType = NotificationBodyType.ApplicationResubmitted;

        // when
        await TestCandidate.Handle(new LoanApplicationHasBeenResubmittedEvent(), CancellationToken.None);

        // then
        notificationServiceMock
            .Verify(
                x =>
                    x.NotifySuccess(applicationResubmittedBodyType, null),
                Times.Once);
    }
}
