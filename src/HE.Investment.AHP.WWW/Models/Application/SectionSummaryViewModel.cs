using HE.Investments.Common.WWW.Components.SectionSummary;

namespace HE.Investment.AHP.WWW.Models.Application;

public record SectionSummaryViewModel(string Title, IList<SectionSummaryItemModel>? Items)
{
    public static SectionSummaryViewModel New(string title, params SectionSummaryItemModel?[] items)
    {
        return new(title, items.Where(x => x != null).Select(x => x!).ToList());
    }
}
