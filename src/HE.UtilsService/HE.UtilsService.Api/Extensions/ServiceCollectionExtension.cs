using HE.UtilsService.Api.Configuration;
using HE.UtilsService.SharePoint.Configuration;
using Microsoft.Extensions.Options;

namespace HE.UtilsService.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddConfigs(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfig>(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
        services.AddSingleton<ISharePointConfiguration>(x => x.GetRequiredService<IAppConfig>().SharePoint);
    }
}
