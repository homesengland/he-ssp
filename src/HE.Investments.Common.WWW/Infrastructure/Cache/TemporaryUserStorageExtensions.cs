using HE.Investments.Common.Infrastructure.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.WWW.Infrastructure.Cache;

public static class TemporaryUserStorageExtensions
{
    public static IServiceCollection AddTemporaryUserStorage(this IServiceCollection services)
    {
        services.AddScoped<ITemporaryUserStorage>(x => new SessionUserStorage(x.GetRequiredService<IHttpContextAccessor>().HttpContext!.Session));

        return services;
    }
}
