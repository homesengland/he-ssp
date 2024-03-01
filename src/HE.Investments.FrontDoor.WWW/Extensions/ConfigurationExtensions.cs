namespace HE.Investments.FrontDoor.WWW.Extensions;

public static class ConfigurationExtensions
{
    public static string? TryGetValue(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<string>($"AppConfiguration:{key}");
    }
}
