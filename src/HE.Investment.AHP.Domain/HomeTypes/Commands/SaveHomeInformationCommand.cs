namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveHomeInformationCommand(
    string ApplicationId,
    string HomeTypeId,
    string? NumberOfHomes,
    string? NumberOfBedrooms,
    string? MaximumOccupancy,
    string? NumberOfStoreys) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
