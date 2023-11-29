using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace HE.Investments.Common.WWWTestsFramework;

public abstract class ViewComponentTestBase<TTestClass> : ViewTestBase
{
    protected override Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = default,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        return base.Render(
            viewPath,
            model,
            viewBagOrViewData,
            modelStateDictionary,
            routeData,
            services =>
            {
                var assembly = typeof(TTestClass).Assembly;
                services.Configure<MvcRazorRuntimeCompilationOptions>(
                    options => options.FileProviders.Add(new EmbeddedFileProvider(assembly)));

                mockDependencies?.Invoke(services);
            });
    }
}
