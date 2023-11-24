using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeTypeDetails(string Id, string Name, int? NumberOfHomes, HousingType HousingType);
