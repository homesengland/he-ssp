using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;

namespace HE.Investments.Account.IntegrationTests.Extensions;

public static class HtmlDocumentExtensions
{
    public static IHtmlAnchorElement GetManageUserLink(this IHtmlTableRowElement userRow)
    {
        var link = userRow.Cells.Last().FindChild<IHtmlAnchorElement>();
        link.Should().NotBeNull();

        return link!;
    }
}
