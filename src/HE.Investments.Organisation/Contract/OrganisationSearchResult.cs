namespace HE.Investments.Organisation.Contract;

public record OrganisationSearchResult(IEnumerable<OrganisationSearchItem> Items, int TotalItems, string Error)
{
    public bool IsSuccessfull() => string.IsNullOrEmpty(Error);

    public static OrganisationSearchResult Empty() => new(Enumerable.Empty<OrganisationSearchItem>(), 0, null!);

    public static OrganisationSearchResult FromSingleItem(OrganisationSearchItem item) => new(new List<OrganisationSearchItem> { item }, 1, null!);
}
