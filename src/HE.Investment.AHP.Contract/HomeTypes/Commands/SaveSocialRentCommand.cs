using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveSocialRentCommand(
    AhpApplicationId ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? ProspectiveRent) : ISaveHomeTypeSegmentCommand;
