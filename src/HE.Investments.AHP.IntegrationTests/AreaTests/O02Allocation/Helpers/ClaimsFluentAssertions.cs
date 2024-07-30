using AngleSharp.Html.Dom;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Helpers;

public static class ClaimsFluentAssertions
{
    public static IHtmlDocument HasGrantDetails(this IHtmlDocument document, decimal totalGrantAllocated, decimal totalGrantRequested, decimal amountRemaining)
    {
        return document
            .HasElementWithTestIdAndText("grant-details-total-grant-allocated", CurrencyHelper.DisplayPounds(totalGrantAllocated))
            .HasElementWithTestIdAndText("grant-details-amount-paid", CurrencyHelper.DisplayPounds(totalGrantRequested))
            .HasElementWithTestIdAndText("grant-details-amount remaining", CurrencyHelper.DisplayPounds(amountRemaining));
    }
}
