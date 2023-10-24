using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.InvestmentLoans.Common.Infrastructure;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHousingTypeCommand).Assembly));
        service.AddScoped<NonceModel>();
        service.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());

        service.AddDomainModule();
    }
}

