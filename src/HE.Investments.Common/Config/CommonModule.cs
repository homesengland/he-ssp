using System.Reflection;
using HE.Investments.Common.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.Config;

public static class CommonModule
{
    public static void AddNotifications(
        this IServiceCollection serviceCollections,
        Assembly notificationMappersAssembly)
    {
        serviceCollections.AddScoped<INotificationService, NotificationService>();

        foreach (var mapperType in notificationMappersAssembly.GetTypes()
                     .Where(x => typeof(INotificationDisplayMapper).IsAssignableFrom(x) && x.IsClass && !x.IsGenericType))
        {
            serviceCollections.AddScoped(typeof(INotificationDisplayMapper), mapperType);
        }
    }
}
