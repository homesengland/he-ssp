using HE.Investment.AHP.WWW.Tests.Helpers;
using HE.Investment.AHP.WWW.Tests.Views;
using HE.Investments.Common.WWW.Routing;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Tests.Shared;
#pragma warning restore CA1716

public class BreadcrumbsTests : ViewTestBase
{
    private readonly string _viewPath = "~/Partials/_Breadcrumbs.cshtml";

    [Fact]
    public async Task ShouldDisplayBreadcrumbs()
    {
        // given
        var model = new List<Breadcrumb> { new("One", "A1", "C1"), new("Two"), new("Three", "A3", "C3") };
        var viewBagOrViewData = new Dictionary<string, object> { { "Breadcrumbs", model } };

        // when
        var document = await Render(_viewPath, model, viewBagOrViewData);

        // then
        document
            .HasElementWithText("a", $"One")
            .HasElementWithText("a", "Two", false)
            .HasElementWithText("span", "Two")
            .HasElementWithText("a", "Three", false)
            .HasElementWithText("span", "Three");
    }

    [Fact]
    public async Task ShouldNotDisplayBreadcrumbs_ForMissingBreadcrumbs()
    {
        // given && when
        var document = await Render<object>(_viewPath);

        // then
        document.IsEmpty();
    }

    [Fact]
    public async Task ShouldNotDisplayBreadcrumbs_ForEmptyBreadcrumbs()
    {
        // given
        var viewBagOrViewData = new Dictionary<string, object> { { "Breadcrumbs", new List<Breadcrumb>() } };

        // when
        var document = await Render<object>(_viewPath, null, viewBagOrViewData);

        // then
        document.IsEmpty();
    }
}
