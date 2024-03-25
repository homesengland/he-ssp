using System.Globalization;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Notifications;
using HE.Investments.Loans.Contract.Application.Events;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;

public class SendApplicationFilesUploadedNotificationEventHandler : IEventHandler<ApplicationFilesUploadedSuccessfullyEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendApplicationFilesUploadedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(ApplicationFilesUploadedSuccessfullyEvent domainEvent, CancellationToken cancellationToken)
    {
        var notification = new ApplicationFilesUploadedSuccessfullyNotification(domainEvent.FilesCount.ToString(CultureInfo.InvariantCulture));
        await _notificationPublisher.Publish(notification);
    }
}
