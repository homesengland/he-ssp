namespace HE.Investments.Organisation.Contract;

public record OrganisationSearchResult(IList<OrganisationSearchItem> Items, int TotalItems, string? Error = null)
{
    public bool IsSuccessful() => string.IsNullOrEmpty(Error);

    public static OrganisationSearchResult Empty() => new(Array.Empty<OrganisationSearchItem>(), 0);

    public static OrganisationSearchResult FromSingleItem(OrganisationSearchItem item) => new(new List<OrganisationSearchItem> { item }, 1);
}
