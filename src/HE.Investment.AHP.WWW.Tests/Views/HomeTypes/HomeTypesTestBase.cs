using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Config;
using HE.Investments.Common.WWWTestsFramework;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public abstract class HomeTypesTestBase : ViewTestBase
{
    protected const string ErrorMessage = "Some error message";

    protected async Task<IHtmlDocument> RenderHomeTypePage<TModel>(
        string viewPath,
        TModel? model = null,
        Dictionary<string, object>? viewBagOrViewData = null,
        ModelStateDictionary? modelStateDictionary = null,
        RouteData? routeData = null,
        Action<IServiceCollection>? mockDependencies = null)
        where TModel : class
    {
        return await Render(
            viewPath,
            model,
            viewBagOrViewData,
            modelStateDictionary,
            new RouteData(
                new RouteValueDictionary { { "applicationId", "123" }, { "homeTypeId", "456" }, }),
            services =>
            {
                services.AddTransient(_ => new Mock<IExternalLinks>().Object);
            });
    }

    protected void AssertErrors(IHtmlDocument document, string fieldName, bool hasError)
    {
        document.HasSummaryErrorMessage(fieldName, ErrorMessage, hasError)
            .HasErrorMessage(fieldName, ErrorMessage, hasError);
    }
}
