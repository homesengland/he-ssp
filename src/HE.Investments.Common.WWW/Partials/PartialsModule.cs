using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HE.Investments.Common.WWW.Partials;

public static class PartialsModule
{
    public static IServiceCollection AddCommonBuildingBlocks(this IServiceCollection services)
    {
        var assembly = typeof(AssemblyMarkup).Assembly;
        services.AddControllersWithViews()
            .AddApplicationPart(assembly)
            .AddRazorOptions(options => options.ViewLocationFormats.Add("/{0}.cshtml"))
            .AddRazorRuntimeCompilation();

        services.Configure<MvcRazorRuntimeCompilationOptions>(
            options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));

        return services;
    }
}
