using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Razor.Templating.Core;

namespace HE.Investments.Common.WWWTestsFramework.Framework;

internal sealed class CustomRazorTemplateEngineRenderer : IRazorTemplateEngine
{
    private readonly IServiceProvider _serviceProvider;

    public CustomRazorTemplateEngineRenderer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<string> RenderAsync(string viewName, object? viewModel = null, Dictionary<string, object>? viewBagOrViewData = null)
    {
        if (string.IsNullOrWhiteSpace(viewName))
        {
            throw new ArgumentNullException(nameof(viewName));
        }

        var viewDataDictionary = GetViewDataDictionaryFromViewBagOrViewData(viewBagOrViewData);

        using var serviceScope = _serviceProvider.CreateScope();
        var renderer = serviceScope.ServiceProvider.GetRequiredService<CustomRazorViewToStringRenderer>();
        return await renderer.RenderViewToStringAsync(viewName, viewModel, viewDataDictionary, null, isMainPage: true).ConfigureAwait(false);
    }

    public async Task<string> RenderWithModelStateDictionaryAsync(string viewName, object? viewModel = null, Dictionary<string, object>? viewBagOrViewData = null, ModelStateDictionary? modelStateDictionary = null)
    {
        if (string.IsNullOrWhiteSpace(viewName))
        {
            throw new ArgumentNullException(nameof(viewName));
        }

        var viewDataDictionary = GetViewDataDictionaryFromViewBagOrViewData(viewBagOrViewData, modelStateDictionary);

        using var serviceScope = _serviceProvider.CreateScope();
        var renderer = serviceScope.ServiceProvider.GetRequiredService<CustomRazorViewToStringRenderer>();
        return await renderer.RenderViewToStringAsync(viewName, viewModel, viewDataDictionary, isMainPage: true).ConfigureAwait(false);
    }

    public async Task<string> RenderPartialAsync(string viewName, object? viewModel = null, Dictionary<string, object>? viewBagOrViewData = null)
    {
        if (string.IsNullOrWhiteSpace(viewName))
        {
            throw new ArgumentNullException(nameof(viewName));
        }

        var viewDataDictionary = GetViewDataDictionaryFromViewBagOrViewData(viewBagOrViewData);

        using var serviceScope = _serviceProvider.CreateScope();
        var renderer = serviceScope.ServiceProvider.GetRequiredService<CustomRazorViewToStringRenderer>();
        return await renderer.RenderViewToStringAsync(viewName, viewModel, viewDataDictionary, isMainPage: false).ConfigureAwait(false);
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public async Task<string> RenderPartialWithModelStateDictionaryAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null)
    {
        if (string.IsNullOrWhiteSpace(viewName))
        {
            throw new ArgumentNullException(nameof(viewName));
        }

        var viewDataDictionary = GetViewDataDictionaryFromViewBagOrViewData(viewBagOrViewData, modelStateDictionary);

        using var serviceScope = _serviceProvider.CreateScope();
        var renderer = serviceScope.ServiceProvider.GetRequiredService<CustomRazorViewToStringRenderer>();
        return await renderer.RenderViewToStringAsync(viewName, viewModel, viewDataDictionary, routeData, isMainPage: false).ConfigureAwait(false);
    }

    private static ViewDataDictionary GetViewDataDictionaryFromViewBagOrViewData(
        Dictionary<string, object>? viewBagOrViewData,
        ModelStateDictionary? modelStateDictionary = null)
    {
        var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelStateDictionary ?? new ModelStateDictionary());

        foreach (var keyValuePair in viewBagOrViewData ?? [])
        {
            viewDataDictionary.Add(keyValuePair!);
        }

        return viewDataDictionary;
    }
}
