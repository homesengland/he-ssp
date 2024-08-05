using HE.DocumentService.SharePoint.Interfaces;
using HE.DocumentService.SharePoint.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HE.DocumentService.SharePoint.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddSharePointServices(this IServiceCollection services)
    {
        services.AddScoped<ISharePointContext, SharePointContext>();
        services.AddScoped<ISharePointFilesService, SharePointFilesService>();
        services.AddScoped<ISharePointFolderService, SharePointFolderService>();
    }
}
