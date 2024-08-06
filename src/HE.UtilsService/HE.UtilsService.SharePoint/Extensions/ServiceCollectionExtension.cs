using HE.UtilsService.SharePoint.Interfaces;
using HE.UtilsService.SharePoint.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HE.UtilsService.SharePoint.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddSharePointServices(this IServiceCollection services)
    {
        services.AddScoped<ISharePointContext, SharePointContext>();
        services.AddScoped<ISharePointFilesService, SharePointFilesService>();
        services.AddScoped<ISharePointFolderService, SharePointFolderService>();
    }
}
