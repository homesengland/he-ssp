using HE.Investments.Common.Models.App;
using Microsoft.Extensions.Options;

namespace HE.Investments.Loans.WWW.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddConfigs(this IServiceCollection services)
    {
        services.AddSingleton<IAppConfig>(x => x.GetRequiredService<IOptions<AppConfig>>().Value);
        services.AddSingleton<IDataverseConfig>(x => x.GetRequiredService<IAppConfig>().Dataverse);
        services.AddSingleton<IUrlConfig>(x => x.GetRequiredService<IAppConfig>().Url);
    }
}
