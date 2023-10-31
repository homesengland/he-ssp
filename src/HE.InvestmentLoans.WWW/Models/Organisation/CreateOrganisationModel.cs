namespace HE.InvestmentLoans.WWW.Models.Organisation;

public record CreateOrganisationModel(
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode);
