using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Contract.Models;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;

namespace HE.InvestmentLoans.Common.Contract.Services;
public class NotificationService : INotificationService
{
    private readonly ICacheService _cacheService;
    private readonly IUserContext _userContext;

    public NotificationService(ICacheService cacheService, IUserContext userContext)
    {
        _cacheService = cacheService;
        _userContext = userContext;
    }

    public string UserGlobalId => _userContext.UserGlobalId;

    public Tuple<bool, NotificationModel?> Pop()
    {
        var key = $"{NotificationServiceKey.Notification}-{UserGlobalId}";
        var isInCache = false;
        var valueFromCache = _cacheService.GetValue<NotificationModel?>(key);

        if (valueFromCache != null)
        {
            isInCache = true;
            _cacheService.SetValue<NotificationModel?>(key, null);
        }

        return Tuple.Create(isInCache, valueFromCache);
    }

    public void NotifySuccess(NotificationBodyType notificationBodyType, string valueToDisplay)
    {
        var key = $"{NotificationServiceKey.Notification}-{UserGlobalId}";
        var notificationModel = new NotificationModel(NotificationTitle.Success, NotificationType.Success, notificationBodyType, valueToDisplay);

        _cacheService.SetValue(key, notificationModel);
    }
}
