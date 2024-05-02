namespace HE.Investments.Organisation.Contract;

public record OrganisationSearchResult(IList<OrganisationSearchItem> Items, int TotalItems, string? Error = null)
{
    public bool IsSuccessful() => string.IsNullOrEmpty(Error);

    public static OrganisationSearchResult Empty() => new([], 0);

    public static OrganisationSearchResult FromSingleItem(OrganisationSearchItem item) => new([item], 1);
}
