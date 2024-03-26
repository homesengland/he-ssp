using HE.Investments.Common;
using HE.Investments.FrontDoor.Shared.Project.Crm;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Shared.Config;

public static class FrontDoorSharedModule
{
    public static void AddFrontDoorSharedModule(this IServiceCollection services)
    {
        services.AddScoped<IProjectCrmContext, ProjectCrmContext>();
        services.AddScoped<IPrefillDataRepository, PrefillDataRepository>();
        services.AddSingleton<IFrontDoorProjectEnumMapping>(x =>
        {
            var featureManager = x.GetRequiredService<IFeatureManager>();
            return featureManager.IsEnabledAsync(FeatureFlags.UseExternalFrontDoorTables).GetAwaiter().GetResult()
                ? new ExternalFrontDoorProjectEnumMapping()
                : new InternalFrontDoorProjectEnumMapping();
        });
    }
}
