using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveHomeInformationCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    string? NumberOfHomes,
    string? NumberOfBedrooms,
    string? MaximumOccupancy,
    string? NumberOfStoreys) : ISaveHomeTypeSegmentCommand;
