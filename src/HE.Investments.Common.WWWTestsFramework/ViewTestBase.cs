using AngleSharp;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.WWWTestsFramework.Framework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investments.Common.WWWTestsFramework;

public abstract class ViewTestBase
{
    protected async Task<IHtmlDocument> Render<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        CustomRazorTemplateEngine.RegisterDependencies = services =>
        {
            services.AddTransient<INotificationService>(_ => new Mock<INotificationService>().Object);
            mockDependencies?.Invoke(services);
        };

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
