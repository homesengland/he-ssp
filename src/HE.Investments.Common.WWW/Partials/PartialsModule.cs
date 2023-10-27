using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HE.Investments.Common.WWW.Partials;

public static class PartialsModule
{
    public static IServiceCollection AddCommonPartialsViews(this IServiceCollection services)
    {
        var assembly = typeof(AssemblyMarkup).Assembly;
        services.AddControllersWithViews()
            .AddApplicationPart(assembly)
            .AddRazorRuntimeCompilation();

        services.Configure<MvcRazorRuntimeCompilationOptions>(
            options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));

        return services;
    }
}
