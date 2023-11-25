namespace HE.Investments.Common.WWW.Components.SectionSummary;

public record SectionSummaryItemModel(string Name, IList<string>? Values, string? ActionUrl, Dictionary<string, string>? Files = null, bool IsEditable = true, bool IsVisible = true);
