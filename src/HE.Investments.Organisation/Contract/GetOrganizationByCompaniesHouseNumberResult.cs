namespace HE.Investments.Organisation.Contract;
public record GetOrganizationByCompaniesHouseNumberResult(OrganisationSearchItem? Item, string? Error = null)
{
    public bool IsSuccessfull() => string.IsNullOrEmpty(Error);
}
