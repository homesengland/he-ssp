using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.EventHandlersTests;

public class LoanApplicationHasBeenResubmittedEventHandlerTests : TestBase<LoanApplicationHasBeenResubmittedEventHandler>
{
    [Fact]
    public async Task ShouldPublishLoanApplicationHasBeenResubmittedNotification()
    {
        // given
        var notificationPublisherMock = CreateAndRegisterDependencyMock<INotificationPublisher>();

        // when
        await TestCandidate.Handle(new LoanApplicationHasBeenResubmittedEvent(new LoanApplicationId(Guid.Empty)), CancellationToken.None);

        // then
        notificationPublisherMock.Verify(x => x.Publish(It.IsAny<LoanApplicationHasBeenResubmittedNotification>()), Times.Once);
    }
}
