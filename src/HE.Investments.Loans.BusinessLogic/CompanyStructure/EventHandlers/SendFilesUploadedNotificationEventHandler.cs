using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.Contract.CompanyStructure.Events;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.EventHandlers;

public class SendFilesUploadedNotificationEventHandler : IEventHandler<FilesUploadedSuccessfullyEvent>
{
    private readonly INotificationPublisher _notificationPublisher;

    public SendFilesUploadedNotificationEventHandler(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async Task Handle(FilesUploadedSuccessfullyEvent domainEvent, CancellationToken cancellationToken)
    {
        var notification = new FilesUploadedSuccessfullyNotification(string.Join(", ", domainEvent.FileNames));
        await _notificationPublisher.Publish(notification);
    }
}
