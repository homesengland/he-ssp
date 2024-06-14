using HE.Investments.Api.Extensions;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Repositories;
using HE.Investments.FrontDoor.Shared.Project.Storage;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;
using HE.Investments.FrontDoor.Shared.Project.Storage.Crm;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.Shared.Config;

public static class FrontDoorSharedModule
{
    public static void AddFrontDoorSharedModule(this IServiceCollection services)
    {
        services.AddConditionalApiContext<IProjectContext, ProjectCrmContext, ProjectApiContext, ProjectContextSelectorDecorator>();
        services.AddScoped<IPrefillDataRepository, PrefillDataRepository>();
    }
}
