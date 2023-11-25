using AngleSharp.Html.Dom;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Xunit;

namespace HE.Investments.Common.WWW.Tests.Components.SectionSummaryItemTests;

public class SectionSummaryItemTests : ViewComponentTestBase
{
    private const string ViewPath = "/Components/SectionSummaryItemTests/TestView.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model);
    }

    [Fact]
    public async Task ShouldNotDisplayView()
    {
        // given
        var model = CreateTestModel(isVisible: false);

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model, false, false, false, false, false);
    }

    [Fact]
    public async Task ShouldDisplayView_ForNotEditable()
    {
        // given
        var model = CreateTestModel(isEditable: false);

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model, isEditable: false);
    }

    [Fact]
    public async Task ShouldDisplayView_ForMissingValue()
    {
        // given
        var model = CreateTestModel(values: new List<string>());

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model, isValueVisible: false);
    }

    private static SectionSummaryItemModel CreateTestModel(
        string name = "some name",
        IList<string>? values = null,
        string actionUrl = "actionUrl",
        Dictionary<string, string>? files = null,
        bool isEditable = true,
        bool isVisible = true)
    {
        values ??= new List<string> { "test text message", "second message" };
        files ??= new Dictionary<string, string> { { "some.pdf", "url" }, { "example.doc", "url" } };

        return new SectionSummaryItemModel(name, values, actionUrl, files, isEditable, isVisible);
    }

    private static void AssertSummaryItem(
        IHtmlDocument document,
        SectionSummaryItemModel model,
        bool isVisible = true,
        bool isNameVisible = true,
        bool isValueVisible = true,
        bool isFileVisible = true,
        bool isEditable = true)
    {
        document
            .HasElementWithText("dt", model.Name, isNameVisible && isVisible)
            .HasElementWithText("p", "Not provided", !isValueVisible && isVisible)
            .HasElementWithText("a", "Change", isEditable && isVisible);

        if (isValueVisible)
        {
            document
                .HasElementWithText("p", model.Values![0]!, isValueVisible && isVisible)
                .HasElementWithText("p", model.Values![1]!, isValueVisible && isVisible);
        }

        if (isFileVisible)
        {
            document
                .HasElementWithText("a", model.Files!.First().Key, isFileVisible && isVisible)
                .HasElementWithText("a", model.Files!.Skip(1).First().Key, isFileVisible && isVisible);
        }
    }
}
