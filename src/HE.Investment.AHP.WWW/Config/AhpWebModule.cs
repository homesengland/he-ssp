using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;
using HE.InvestmentLoans.Common.Infrastructure;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service, WebApplicationBuilder builder)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHomeTypeDetailsCommandHandler).Assembly));
        service.AddScoped<NonceModel>();
        service.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());

        service.Configure<ContactInfoOptions>(builder.Configuration.GetSection("AppConfiguration:ContactInfo"));

        service.AddDomainModule();
    }
}
