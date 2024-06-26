using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.WWW.Components.SectionSummary;

namespace HE.Investments.Common.WWW.Tests.Components.SectionSummaryItemTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "ViewComponents tests are failing on CI from time to time.")]
public class SectionSummaryItemTests : ViewComponentTestBase<SectionSummaryItemTests>
{
    private const string ViewPath = "/Components/SectionSummaryItemTests/SectionSummaryItemTests.cshtml";

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayView()
    {
        // given
        var model = CreateTestModel();

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldNotDisplayView()
    {
        // given
        var model = CreateTestModel(isVisible: false);

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model, false, false, false, false, false);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayView_ForNotEditable()
    {
        // given
        var model = CreateTestModel(isEditable: false);

        // when
        var document = await Render(ViewPath, model);

        // then
        AssertSummaryItem(document, model, isEditable: false);
    }

    [Fact(Skip = Constants.SkipTest)]
    public async Task ShouldDisplayView_ForMissingValue()
    {
        // given
        var model = CreateTestModel(values: []);

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
        values ??= ["test text message", "second message"];
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
                .HasElementWithText("p", model.Values![0]!, isVisible)
                .HasElementWithText("p", model.Values![1]!, isVisible);
        }

        if (isFileVisible)
        {
            document
                .HasElementWithText("a", model.Files!.First().Key, isVisible)
                .HasElementWithText("a", model.Files!.Skip(1).First().Key, isVisible);
        }
    }
}
