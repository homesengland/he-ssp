using Microsoft.FeatureManagement;

namespace HE.Investments.Common.CRM.Extensions;

public static class FeatureManagerExtensions
{
    public static async Task<string> GetUseHeTablesParameter(this IFeatureManager featureManager)
    {
        return await featureManager.IsEnabledAsync(FeatureFlags.UseExternalFrontDoorTables) ? true.ToString() : string.Empty;
    }
}
