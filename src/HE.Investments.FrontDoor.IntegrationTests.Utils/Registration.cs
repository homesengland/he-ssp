using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.IntegrationTests.Utils;

public static class Registration
{
    public static void AddFrontDoorManipulator(this IServiceCollection services)
    {
        services.AddScoped<FrontDoorDataManipulator>();
        services.AddScoped<IProjectContext, ProjectCrmContext>();
        services.AddScoped<ISiteContext, SiteCrmContext>();
    }
}
