using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeInformation(
    string ApplicationName,
    string HomeTypeName,
    int? NumberOfHomes,
    int? NumberOfBedrooms,
    int? MaximumOccupancy,
    int? NumberOfStoreys,
    YesNoType IntendedAsMoveOnAccommodation);
