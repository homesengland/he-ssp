namespace HE.Investments.AHP.Consortium.Contract.Commands;

public record AddManualOrganisationToConsortiumCommand(
    ConsortiumId ConsortiumId,
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IConsortiumCommand;
