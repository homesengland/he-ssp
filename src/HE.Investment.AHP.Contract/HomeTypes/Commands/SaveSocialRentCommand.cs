using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveSocialRentCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    string? MarketValue,
    string? ProspectiveRent) : ISaveHomeTypeSegmentCommand;
