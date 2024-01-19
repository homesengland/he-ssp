using HE.Investments.Common.WWW.Partials;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Common.WWWTestsFramework.Framework;

public static class CustomRazorTemplateEngine
{
    private static readonly Lazy<CustomRazorTemplateEngineRenderer> Instance = new(CreateInstance, true);
    private static IServiceCollection? _services;

    public static Action<IServiceCollection>? RegisterDependencies { get; set; }

    public static async Task<string> RenderAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null)
    {
        return await Instance.Value.RenderWithModelStateDictionaryAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary).ConfigureAwait(false);
    }

    public static async Task<string> RenderPartialAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null)
    {
        return await Instance.Value.RenderPartialWithModelStateDictionaryAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary, routeData).ConfigureAwait(false);
    }

    private static CustomRazorTemplateEngineRenderer CreateInstance()
    {
        if (_services is null)
        {
            _services = new ServiceCollection();
            _services.AddRazorTemplating();

            _services.AddTransient<CustomRazorViewToStringRenderer>();
            _services.AddTransient<CustomRazorTemplateEngineRenderer>();

            _services.AddCommonBuildingBlocks();
        }

        RegisterDependencies?.Invoke(_services);

        return _services.BuildServiceProvider().GetRequiredService<CustomRazorTemplateEngineRenderer>();
    }
}
