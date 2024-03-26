using HE.Investments.Common.CRM.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace HE.Investments.Common.CRM.Config;

public static class CommonCrmModule
{
    public static void AddCommonCrmModule(this IServiceCollection services)
    {
        services.AddSingleton<IPlanningStatusMapper>(x =>
        {
            var featureManager = x.GetRequiredService<IFeatureManager>();
            return featureManager.IsEnabledAsync(FeatureFlags.UseExternalFrontDoorTables).GetAwaiter().GetResult()
                ? new ExternalPlanningStatusMapper()
                : new InternalPlanningStatusMapper();
        });
    }
}
