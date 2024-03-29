using AngleSharp;
using AngleSharp.Html.Dom;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.WWWTestsFramework.Framework;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Moq;

namespace HE.Investments.Common.WWWTestsFramework;

public abstract class ViewTestBase
{
    protected virtual async Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        CustomRazorTemplateEngine.RegisterDependencies = services =>
        {
            services.AddTransient(_ => new Mock<INotificationConsumer>().Object);
            services.AddTransient(_ => new Mock<IAccountAccessContext>().Object);
            services.AddTransient(_ => new Mock<IFeatureManager>().Object);
            mockDependencies?.Invoke(services);
        };

        var html = await CustomRazorTemplateEngine.RenderPartialAsync(viewPath, model, viewBagOrViewData, modelStateDictionary, routeData);

        var document = await BrowsingContext.New().OpenAsync(r => r.Content(html), CancellationToken.None);
        return document as IHtmlDocument ?? throw new InvalidOperationException();
    }
}
