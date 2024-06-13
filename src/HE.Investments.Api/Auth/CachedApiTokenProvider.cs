using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Utils;

namespace HE.Investments.Api.Auth;

internal sealed class CachedApiTokenProvider : IApiTokenProvider
{
    private const string ApiTokenCacheKey = "investments-api-token";

    private const int InvalidateCachedTokenTimeoutInMinutes = 5;

    private readonly ApiTokenProvider _tokenProvider;

    private readonly ICacheService _cacheService;

    private readonly IDateTimeProvider _dateTimeProvider;

    private ApiTokenProvider.ApiAccessToken? _token;

    public CachedApiTokenProvider(ApiTokenProvider tokenProvider, ICacheService cacheService, IDateTimeProvider dateTimeProvider)
    {
        _tokenProvider = tokenProvider;
        _cacheService = cacheService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<string> GetToken()
    {
        _token ??= await _cacheService.GetValueAsync(ApiTokenCacheKey, async () => await _tokenProvider.GetToken());
        if (_token!.ExpiresOn.AddMinutes(-InvalidateCachedTokenTimeoutInMinutes) <= new DateTimeOffset(_dateTimeProvider.UtcNow, TimeSpan.Zero))
        {
            await _cacheService.DeleteAsync(ApiTokenCacheKey);
        }

        return _token!.AccessToken;
    }
}
