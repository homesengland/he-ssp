namespace HE.Investments.Organisation.Contract;

public record OrganisationSearchResult(IEnumerable<OrganisationSearchItem> Items, int TotalItems, string Error)
{
    public bool IsSuccessfull() => string.IsNullOrEmpty(Error);
}
