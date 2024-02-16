using System.Reflection;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.User;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.Config;

public static class CommonModule
{
    public static void AddNotifications(
        this IServiceCollection serviceCollections,
        string applicationName,
        Assembly displayNotificationFactoriesAssembly)
    {
        serviceCollections.AddScoped<INotificationService>(x => new NotificationService(
            x.GetRequiredService<ICacheService>(),
            x.GetRequiredService<IUserContext>(),
            x.GetServices<IDisplayNotificationFactory>(),
            applicationName));

        foreach (var factoryType in displayNotificationFactoriesAssembly.GetTypes()
                     .Where(x => typeof(IDisplayNotificationFactory).IsAssignableFrom(x) && x.IsClass && !x.IsGenericType))
        {
            serviceCollections.AddScoped(typeof(IDisplayNotificationFactory), factoryType);
        }
    }
}
