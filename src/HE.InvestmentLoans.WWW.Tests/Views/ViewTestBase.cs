using AngleSharp;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Razor.Templating.Core;
using NotificationModel = HE.InvestmentLoans.Common.Contract.Models.NotificationModel;

namespace HE.InvestmentLoans.WWW.Tests.Views;

public abstract class ViewTestBase
{
    protected async Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        Action<ServiceCollection>? mockDependencies = null)
    where TModel : class
    {
        var notificationServiceMock = new Mock<INotificationService>();
        notificationServiceMock
            .Setup(s => s.Pop())
            .Returns(Tuple.Create(false, (NotificationModel)null!)!);

        var services = new ServiceCollection();
        services.AddTransient<INotificationService>(_ => notificationServiceMock.Object);
        mockDependencies?.Invoke(services);
        services.AddRazorTemplating();

        var html = await RazorTemplateEngine.RenderPartialAsync(viewPath, model, viewBagOrViewData);

        var document = await BrowsingContext.New().OpenAsync(r => r.Content(html), CancellationToken.None);
        return document as IHtmlDocument ?? throw new InvalidOperationException();
    }
}
