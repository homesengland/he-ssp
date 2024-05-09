namespace HE.Investments.Common.WWW.Components.SummaryList;

public record SummaryListItem(string Name, IList<SummaryListItemAction> Actions)
{
    public SummaryListItem(string name)
        : this(name, [])
    {
    }
}
