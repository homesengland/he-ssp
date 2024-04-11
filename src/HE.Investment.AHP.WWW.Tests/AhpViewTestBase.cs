using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Config;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investment.AHP.WWW.Tests;

public abstract class AhpViewTestBase : ViewTestBase
{
    protected override async Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        var document = await base.Render(viewPath, model, viewBagOrViewData, modelStateDictionary, routeData, services =>
        {
            services.AddSingleton(_ => new Mock<IAhpExternalLinks>().Object);
            services.AddSingleton(_ => new Mock<ContactInfo>().Object);
        });

        return document;
    }
}
