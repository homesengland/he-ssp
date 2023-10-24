using HE.Investment.AHP.BusinessLogic.HomeTypes;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Commands;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Mappers;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.InvestmentLoans.Common.Infrastructure;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHousingTypeCommand).Assembly));
        service.AddScoped<NonceModel>();
        service.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());

        // TODO: move to domain module
        service.AddSingleton<IHomeTypeRepository, HomeTypeRepository>();
        service.AddSingleton<IHomeTypeSectionMapper<HousingTypeSection>, HousingTypeSectionMapper>();
        service.AddHttpContextAccessor();
    }
}

