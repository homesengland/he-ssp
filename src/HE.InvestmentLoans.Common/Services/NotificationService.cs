using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Services.Interfaces;

namespace HE.InvestmentLoans.Common.Services;
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

    public void Add(string key, string applicationName)
    {
        _cacheService.SetValue($"{key}-{UserGlobalId}", applicationName);
    }

    public Tuple<bool, string> Pop(string key)
    {
        var searchKey = $"{key}-{UserGlobalId}";
        var isInCache = false;
        var valueFromCache = _cacheService.GetValue<string>(searchKey) ?? string.Empty;

        if (!string.IsNullOrEmpty(valueFromCache))
        {
            isInCache = true;
            _cacheService.SetValue(searchKey, string.Empty);
        }

        return Tuple.Create(isInCache, valueFromCache);
    }
}
