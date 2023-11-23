using AngleSharp.Html.Dom;
using HE.Investments.Common.WWWTestsFramework;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HE.Investments.Common.WWW.Tests.Components;

public abstract class ViewComponentTestBase : ViewTestBase
{
    protected override Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = default,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        return base.Render(
            viewPath,
            model,
            viewBagOrViewData,
            modelStateDictionary,
            services =>
            {
                var assembly = typeof(ViewComponentTestBase).Assembly;
                services.Configure<MvcRazorRuntimeCompilationOptions>(
                    options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));

                mockDependencies?.Invoke(services);
            });
    }
}
