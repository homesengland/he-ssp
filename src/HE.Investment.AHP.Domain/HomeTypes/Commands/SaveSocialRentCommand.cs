namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveSocialRentCommand(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? MarketRent) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
