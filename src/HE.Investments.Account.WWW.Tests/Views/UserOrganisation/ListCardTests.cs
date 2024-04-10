using AngleSharp.Html.Dom;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;

namespace HE.Investments.Account.WWW.Tests.Views.UserOrganisation;

public class ListCardTests : AccountViewTestBase
{
    private readonly string _viewPath = "/Views/UserOrganisation/_ListCard.cshtml";

    [Fact]
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

    [Fact]
    public async Task ShouldDisplayListCard_WhenThereAreLessThan5Applications()
    {
        // given
        var model = CreateTestModel(new[]
        {
            new ListCardItemModel("Ap1", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap2", CreateStatusComponent(ApplicationStatus.Draft), "http://localhost/app/"),
            new ListCardItemModel("Ap3", CreateStatusComponent(ApplicationStatus.ApplicationSubmitted), "http://localhost/app/"),
        });

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", $"Applications")
            .HasElementWithText("p", "other(s)", false)
            .HasElementWithText("a", "View all");

        AssertApplications(model, document);
    }

    [Fact]
    public async Task ShouldNotDisplayListCard_WhenThereAreNoApplications()
    {
        // given
        var model = CreateTestModel(new List<ListCardItemModel>());

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasElementWithText("h3", $"Applications", false)
            .HasElementWithText("p", "other(s)", false)
            .HasElementWithText("a", "View all", false);
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
        return new[]
        {
            new ListCardItemModel("Ap1", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap2", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap3", CreateStatusComponent(ApplicationStatus.ApplicationDeclined), "http://localhost/app/"),
            new ListCardItemModel("Ap4", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap5", CreateStatusComponent(ApplicationStatus.Draft), "http://localhost/app/"),
            new ListCardItemModel("Ap6", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
            new ListCardItemModel("Ap7", CreateStatusComponent(ApplicationStatus.New), "http://localhost/app/"),
        };
    }

    private static DynamicComponentViewModel CreateStatusComponent(ApplicationStatus status)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { ApplicationStatus = status });
    }

    private ListCardModel CreateTestModel(IList<ListCardItemModel>? applications = null)
    {
        return new ListCardModel(
            "Nagłówek",
            applications ?? ApplicationBasicDetailsModels(),
            Title: "Applications",
            ViewAllUrl: "applications/");
    }
}
