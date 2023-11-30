using HE.Investments.Common.WWW.Components.SectionSummary;

namespace HE.Investment.AHP.WWW.Models.Application;

public record SectionSummaryViewModel(string Title, IList<SectionSummaryItemModel>? Items);
