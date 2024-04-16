using HE.Investments.FrontDoor.Shared.Project.Crm;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.Shared.Config;

public static class FrontDoorSharedModule
{
    public static void AddFrontDoorSharedModule(this IServiceCollection services)
    {
        services.AddScoped<IProjectCrmContext, ProjectCrmContext>();
        services.AddScoped<IPrefillDataRepository, PrefillDataRepository>();
    }
}
