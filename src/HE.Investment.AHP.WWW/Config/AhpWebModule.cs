using HE.InvestmentLoans.Common.Infrastructure;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service)
    {
        service.AddScoped<NonceModel>();
        service.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());
    }
}
