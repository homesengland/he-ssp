namespace HE.Investments.Common.WWW.Components.SectionSummary;

public record SectionSummaryItemModel(string Name, IList<string>? Values, string? ActionUrl = null, Dictionary<string, string>? Files = null, bool IsEditable = true,
    bool IsVisible = true)
{
    public bool HasAnswer => Values?.Any() == true || Files?.Any() == true;

    public bool HasRedirectAction => !string.IsNullOrWhiteSpace(ActionUrl);
}
