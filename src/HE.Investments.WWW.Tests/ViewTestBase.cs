using AngleSharp;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.WWW.Tests.Framework;
using HE.Investments.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NotificationModel = HE.Investments.Common.Services.Notifications.NotificationModel;

namespace HE.Investments.WWW.Tests;

public abstract class ViewTestBase
{
    protected async Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
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

        var html = await CustomRazorTemplateEngine.RenderPartialAsync(viewPath, model, viewBagOrViewData, modelStateDictionary);

        var document = await BrowsingContext.New().OpenAsync(r => r.Content(html), CancellationToken.None);
        return document as IHtmlDocument ?? throw new InvalidOperationException();
    }

    protected void AssertError(IHtmlDocument document, string fieldName, string errorMessage, bool hasError)
    {
        document.HasSummaryErrorMessage(fieldName, errorMessage, hasError)
            .HasErrorMessage(fieldName, errorMessage, hasError);
    }
}
