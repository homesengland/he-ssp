using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;
using HE.Investments.Common.WWW.Components.ListCard;

namespace HE.Investments.Common.WWW.Tests.Components.ListCardTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "ViewComponents tests are failing on CI from time to time.")]
public class ListCardTests : ViewComponentTestBase<ListCardTests>
{
    private readonly string _viewPath = "/Components/ListCardTests/ListCardTests.cshtml";

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayListCard_WhenThereAreMoreThan5Applications()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", "Applications")
            .HasElementWithText("p", "2 other(s)")
            .HasElementWithText("a", "View all");

        AssertApplications(model, document);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayListCard_WhenThereAreLessThan5Applications()
    {
        // given
        var model = CreateTestModel(
        [
            new ListCardItemModel("Ap1", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap2", CreateStatusComponent(ApplicationStatus.Draft), "http://localhost/app/"),
            new ListCardItemModel("Ap3", CreateStatusComponent(ApplicationStatus.ApplicationSubmitted), "http://localhost/app/"),
        ]);

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", "Applications")
            .HasElementWithText("p", "other(s)", false)
            .HasElementWithText("a", "View all");

        AssertApplications(model, document);
    }

    private static void AssertApplications(ListCardModel model, IHtmlDocument document)
    {
        for (var i = 0; i < model.Items.Count; i++)
        {
            document.HasElementWithText("a", model.Items[i].Name, i < 5);
        }
    }

    private static ListCardItemModel[] ApplicationBasicDetailsModels()
    {
        return
        [
            new ListCardItemModel("Ap1", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap2", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap3", CreateStatusComponent(ApplicationStatus.ApplicationDeclined), "http://localhost/app/"),
            new ListCardItemModel("Ap4", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap5", CreateStatusComponent(ApplicationStatus.Draft), "http://localhost/app/"),
            new ListCardItemModel("Ap6", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap7", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
        ];
    }

    private static DynamicComponentViewModel CreateStatusComponent(ApplicationStatus status)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { applicationStatus = status, additionalClasses = string.Empty });
    }

    private ListCardModel CreateTestModel(IList<ListCardItemModel>? applications = null)
    {
        return new ListCardModel(
            "Nagłówek",
            applications ?? ApplicationBasicDetailsModels(),
            Title: "Applications",
            ViewAllLabel: "View all applications",
            ViewAllUrl: "applications/");
    }
}
