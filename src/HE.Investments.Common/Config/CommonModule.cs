using System.Reflection;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HE.Investments.Common.Config;

public static class CommonModule
{
    public static void AddNotificationPublisher(this IServiceCollection services, ApplicationType application)
    {
        services.TryAddScoped<INotificationKeyFactory, NotificationKeyFactory>();
        services.AddScoped<INotificationPublisher>(x =>
            new NotificationPublisher(x.GetRequiredService<ICacheService>(), x.GetRequiredService<INotificationKeyFactory>(), application));
    }

    public static void AddNotificationConsumer(
        this IServiceCollection services,
        ApplicationType application,
        Assembly displayNotificationFactoriesAssembly)
    {
        services.TryAddScoped<INotificationKeyFactory, NotificationKeyFactory>();
        services.AddScoped<INotificationConsumer>(x => new NotificationConsumer(
            x.GetRequiredService<ICacheService>(),
            x.GetRequiredService<INotificationKeyFactory>(),
            x.GetServices<IDisplayNotificationFactory>(),
            application));

        foreach (var factoryType in displayNotificationFactoriesAssembly.GetTypes()
                     .Where(x => typeof(IDisplayNotificationFactory).IsAssignableFrom(x) && x.IsClass && !x.IsGenericType))
        {
            services.AddScoped(typeof(IDisplayNotificationFactory), factoryType);
        }
    }
}
