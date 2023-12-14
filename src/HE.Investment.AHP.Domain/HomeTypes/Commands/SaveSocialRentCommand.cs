namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveSocialRentCommand(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? ProspectiveRent) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
