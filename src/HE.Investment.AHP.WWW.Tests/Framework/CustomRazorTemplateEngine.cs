using HE.Investments.Common.WWW;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HE.Investment.AHP.WWW.Tests.Framework;

public static class CustomRazorTemplateEngine
{
    private static readonly Lazy<CustomRazorTemplateEngineRenderer> Instance = new(CreateInstance, true);
    private static IServiceCollection? _services;

    public static async Task<string> RenderAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null)
    {
        return await Instance.Value.RenderAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary).ConfigureAwait(false);
    }

    public static async Task<string> RenderPartialAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null)
    {
        return await Instance.Value.RenderPartialAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary).ConfigureAwait(false);
    }

    private static CustomRazorTemplateEngineRenderer CreateInstance()
    {
        if (_services is null)
        {
            _services = new ServiceCollection();
            _services.AddRazorTemplating();

            _services.AddTransient<CustomRazorViewToStringRenderer>();
            _services.AddTransient<CustomRazorTemplateEngineRenderer>();

            var assembly = typeof(AssemblyMarkup).Assembly;
            _services.AddControllersWithViews()
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation();
            _services.Configure<MvcRazorRuntimeCompilationOptions>(
                options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));
        }

        return _services.BuildServiceProvider().GetRequiredService<CustomRazorTemplateEngineRenderer>();
    }
}
