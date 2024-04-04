using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.Account.IntegrationTests.Extensions;

public static class OrganisationFluentExtensions
{
    public static IHtmlDocument HasOrganisationSearchResultItem(this IHtmlDocument htmlDocument, string organisationName, out string organisationId)
    {
        htmlDocument.HasElementForTestId($"organisation-{organisationName.ToIdTag()}", out var listItem);
        var organisationLink = listItem.FindDescendant<IHtmlAnchorElement>();
        organisationLink.Should().NotBeNull();
        organisationLink!.GetAttribute("data-testId").Should().NotBeNull();

        organisationId = organisationLink.GetAttribute("data-testId")!.Replace("confirm-organisation-", string.Empty);

        return htmlDocument;
    }

    public static IHtmlDocument HasActionLink(this IHtmlDocument htmlDocument, string href, out IHtmlAnchorElement actionLink)
    {
        var link = htmlDocument
            .GetElementsByTagName("a")
            .OfType<IHtmlAnchorElement>()
            .SingleOrDefault(x => x.Href.Split("?")[0].EndsWith(href, StringComparison.CurrentCultureIgnoreCase));

        link.Should().NotBeNull();
        link!.IsDisabled().Should().BeFalse();
        actionLink = link!;

        return htmlDocument;
    }

    public static IHtmlDocument HasManageUserRow(this IHtmlDocument htmlDocument, string userEmail, out IHtmlTableRowElement rowElement)
    {
        htmlDocument.HasElementForTestId("users-table", out var table);
        var usersTable = table.FindDescendant<IHtmlTableElement>();

        usersTable.Should().NotBeNull();
        var userRow = usersTable!.Rows.FirstOrDefault(x => x.Cells.OfType<IHtmlTableDataCellElement>().Any(y => y.TextContent.Contains(userEmail)));

        userRow.Should().NotBeNull();
        rowElement = userRow!;

        return htmlDocument;
    }
}
