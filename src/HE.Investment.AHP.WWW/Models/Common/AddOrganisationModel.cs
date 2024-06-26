namespace HE.Investment.AHP.WWW.Models.Common;

public record AddOrganisationModel(
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode);
