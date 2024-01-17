using AngleSharp.Html.Dom;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.Extensions;

public static class DeliveryPhaseFluentExtensions
{
    public static IList<string> GetDeliveryPhaseIds(this IHtmlDocument deliveryPhaseListPage)
    {
        return deliveryPhaseListPage.GetHiddenListInput("DeliveryPhases", "DeliveryPhaseId").ToList();
    }

    public static IHtmlDocument HasRemoveDeliveryPhaseLink(this IHtmlDocument htmlDocument, string deliveryPhaseId, out IHtmlAnchorElement removeButton)
    {
        return htmlDocument.HasLinkWithTestId($"remove-{deliveryPhaseId}", out removeButton);
    }
}
