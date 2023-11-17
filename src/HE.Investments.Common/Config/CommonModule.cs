using System.Reflection;
using HE.Investments.Common.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.Config;

public static class CommonModule
{
    public static void AddNotifications(
        this IServiceCollection serviceCollections,
        Assembly displayNotificationFactoriesAssembly)
    {
        serviceCollections.AddScoped<INotificationService, NotificationService>();

        foreach (var factoryType in displayNotificationFactoriesAssembly.GetTypes()
                     .Where(x => typeof(IDisplayNotificationFactory).IsAssignableFrom(x) && x.IsClass && !x.IsGenericType))
        {
            serviceCollections.AddScoped(typeof(IDisplayNotificationFactory), factoryType);
        }
    }

    public static void TestMethod()
    {
    }
}
