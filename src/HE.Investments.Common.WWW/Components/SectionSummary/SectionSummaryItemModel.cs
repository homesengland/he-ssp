namespace HE.Investments.Common.WWW.Components.SectionSummary;

public record SectionSummaryItemModel(string Name, IList<string>? Values, string? ActionUrl = null, Dictionary<string, string>? Files = null, bool IsEditable = true,
    bool IsVisible = true)
{
    public bool HasAnswer => Values?.Count > 0 || Files?.Count > 0;

    public bool HasRedirectAction => !string.IsNullOrWhiteSpace(ActionUrl);
}
