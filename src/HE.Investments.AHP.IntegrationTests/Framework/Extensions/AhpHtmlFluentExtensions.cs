using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Framework.Extensions;

public static class AhpHtmlFluentExtensions
{
    public static IHtmlDocument HasChangeAnswerSummaryButton(this IHtmlDocument htmlDocument, string summaryName, out IHtmlAnchorElement changeAnswerButton)
    {
        var summaryRow = htmlDocument.GetElementByTestId($"{summaryName.ToIdTag()}-summary");
        var changeLink = summaryRow.FindDescendant<IHtmlAnchorElement>();
        changeLink.Should().NotBeNull();

        changeLink!.Text.Trim().Should().Be("Change");
        changeAnswerButton = changeLink!;

        return htmlDocument;
    }

    public static IEnumerable<string> GetHiddenListInput(this IHtmlDocument htmlDocument, string collectionName, string propertyName)
    {
        var index = 0;
        while (true)
        {
            var value = (htmlDocument.GetElementById($"{collectionName}_{index}__{propertyName}") as IHtmlInputElement)?.Value;
            if (!string.IsNullOrEmpty(value))
            {
                yield return value.Trim();
            }
            else
            {
                yield break;
            }

            index++;
        }
    }

    public static IHtmlDocument HasNavigationListItem(this IHtmlDocument htmlDocument, string listTestId, out IHtmlAnchorElement firstNavigationButton)
    {
        var list = htmlDocument.GetElementByTestId(listTestId);
        var link = list.FindDescendant<IHtmlAnchorElement>();
        link.Should().NotBeNull();

        firstNavigationButton = link!;

        return htmlDocument;
    }

    public static IHtmlDocument HasPartnerSelectItems(this IHtmlDocument htmlDocument, out IList<(InvestmentsOrganisation Organisation, IHtmlAnchorElement Link)> items)
    {
        var selectList = htmlDocument.GetElementByTestId("select-list");
        var links = selectList.GetElementsByClassName("govuk-link");

        items = links.OfType<IHtmlAnchorElement>()
            .Select(x => (Organisation: new InvestmentsOrganisation(OrganisationId.From(x.Href.Split("/")[^1]), x.Text().Trim()), Link: x))
            .ToList();

        items.Should().NotBeEmpty();

        return htmlDocument;
    }
}
