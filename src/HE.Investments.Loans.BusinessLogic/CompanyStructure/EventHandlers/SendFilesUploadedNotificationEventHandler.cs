using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.Contract.CompanyStructure.Events;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.EventHandlers;

public class SendFilesUploadedNotificationEventHandler : IEventHandler<FilesUploadedSuccessfullyEvent>
{
    private readonly INotificationService _notificationService;

    public SendFilesUploadedNotificationEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(FilesUploadedSuccessfullyEvent domainEvent, CancellationToken cancellationToken)
    {
        var notification = new FilesUploadedSuccessfullyNotification(string.Join(", ", domainEvent.FileNames));
        await _notificationService.Publish(notification);
    }
}
