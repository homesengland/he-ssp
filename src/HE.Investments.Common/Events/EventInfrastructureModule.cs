using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.Events;

public static class EventInfrastructureModule
{
    public static void AddEventInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher>();
    }
}
