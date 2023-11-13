using HE.Investments.Common.WWW.Partials;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.WWW.Tests.Framework;

public class CustomRazorTemplateEngine
{
    private readonly CustomRazorTemplateEngineRenderer _renderer;

    public CustomRazorTemplateEngine(IServiceCollection? services = null)
    {
        _renderer = CreateInstance(services ?? new ServiceCollection());
    }

    public async Task<string> RenderAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null)
    {
        return await _renderer.RenderAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary).ConfigureAwait(false);
    }

    public async Task<string> RenderPartialAsync(
        string viewName,
        object? viewModel = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null)
    {
        return await _renderer.RenderPartialAsync(viewName, viewModel, viewBagOrViewData, modelStateDictionary).ConfigureAwait(false);
    }

    private CustomRazorTemplateEngineRenderer CreateInstance(IServiceCollection services)
    {
        services.AddRazorTemplating();

        services.AddTransient<CustomRazorViewToStringRenderer>();
        services.AddTransient<CustomRazorTemplateEngineRenderer>();

        services.AddCommonBuildingBlocks();

        return services.BuildServiceProvider().GetRequiredService<CustomRazorTemplateEngineRenderer>();
    }
}
