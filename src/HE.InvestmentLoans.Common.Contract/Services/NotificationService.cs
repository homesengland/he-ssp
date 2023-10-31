using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Contract.Models;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;

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

    private string UserGlobalId => _userContext.UserGlobalId;

    public Tuple<bool, NotificationModel?> Pop()
    {
        var key = $"{NotificationServiceCacheKey.Notification}-{UserGlobalId}";
        var isInCache = false;
        var valueFromCache = _cacheService.GetValue<NotificationModel?>(key);

        if (valueFromCache != null)
        {
            isInCache = true;
            _cacheService.SetValue<NotificationModel?>(key, null);
        }

        return Tuple.Create(isInCache, valueFromCache);
    }

    public async Task NotifySuccess(NotificationBodyType notificationBodyType, IDictionary<NotificationServiceKeys, string>? valuesToDisplay)
    {
        var key = $"{NotificationServiceCacheKey.Notification}-{UserGlobalId}";
        var notificationModel = new NotificationModel(NotificationTitle.Success, NotificationType.Success, notificationBodyType, valuesToDisplay);

        await _cacheService.SetValueAsync(key, notificationModel);
    }
}
