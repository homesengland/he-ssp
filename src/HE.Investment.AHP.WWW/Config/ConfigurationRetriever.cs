namespace HE.Investment.AHP.WWW.Config;

public static class ConfigurationRetriever
{
    public static string? TryGetValue(IConfiguration configuration, string key)
    {
        return configuration.GetValue<string>($"AppConfiguration:{key}");
    }
}
