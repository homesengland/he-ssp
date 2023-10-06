namespace HE.Investments.Organisation.Contract;

public record OrganisationSearchResult(IList<OrganisationSearchItem> Items, int TotalItems, string? Error)
{
    public bool IsSuccessfull() => string.IsNullOrEmpty(Error);

    public static OrganisationSearchResult Empty() => new(Array.Empty<OrganisationSearchItem>(), 0, null!);

    public static OrganisationSearchResult FromSingleItem(OrganisationSearchItem item) => new(new List<OrganisationSearchItem> { item }, 1, null!);
}
