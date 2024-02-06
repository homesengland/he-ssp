using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Account.Contract.UserOrganisation;
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

    public static IHtmlDocument HasOrganisationJoinRequestConfirmation(this IHtmlDocument htmlDocument)
    {
        return htmlDocument.HasElementForTestId("user-organisation-limited-user", out _);
    }

    public static IHtmlDocument HasStartNewApplicationButton(this IHtmlDocument htmlDocument, ProgrammeType programmeType)
    {
        return htmlDocument.HasLinkWithTestId($"user-organisation-start-new-application-{programmeType.ToString().ToLowerInvariant()}", out _);
    }

    public static IHtmlDocument HasActionLink(this IHtmlDocument htmlDocument, string href, out IHtmlAnchorElement actionLink)
    {
        var link = htmlDocument
            .GetElementsByTagName("a")
            .OfType<IHtmlAnchorElement>()
            .SingleOrDefault(x => x.Href.EndsWith(href, StringComparison.CurrentCultureIgnoreCase));

        link.Should().NotBeNull();
        link!.IsDisabled().Should().BeFalse();
        actionLink = link!;

        return htmlDocument;
    }
}
