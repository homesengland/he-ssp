using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppConfiguration<TService, TImplementation>(this IServiceCollection services, string? nestedConfigurationKey = null)
        where TService : class
        where TImplementation : class, TService
    {
        var configurationKey = GetConfigurationKey(nestedConfigurationKey);
        return services.AddSingleton<TService, TImplementation>(x =>
            x.GetRequiredService<IConfiguration>().GetSection(configurationKey).Get<TImplementation>());
    }

    public static IServiceCollection AddAppConfiguration<TImplementation>(this IServiceCollection services, string? nestedConfigurationKey = null)
        where TImplementation : class
    {
        var configurationKey = GetConfigurationKey(nestedConfigurationKey);
        return services.AddSingleton<TImplementation>(x =>
            x.GetRequiredService<IConfiguration>().GetSection(configurationKey).Get<TImplementation>());
    }

    private static string GetConfigurationKey(string? nestedConfigurationKey)
    {
        return string.IsNullOrEmpty(nestedConfigurationKey)
            ? "AppConfiguration"
            : $"AppConfiguration:{nestedConfigurationKey.TrimStart(':')}";
    }
}
