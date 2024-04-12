using AngleSharp.Html.Dom;
using HE.Investments.Account.WWW.Config;
using HE.Investments.Common.Services.Notifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investments.Account.WWW.Tests;

public abstract class AccountViewTestBase : ViewTestBase
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
            services.AddSingleton(_ => new Mock<IAccountExternalLinks>().Object);
            services.AddSingleton(_ => new Mock<INotificationConsumer>().Object);
        });

        return document;
    }
}
