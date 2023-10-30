using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.WWW.Models.HomeTypes;

public record HomeTypeItemModel(string HomeTypeId, string HomeTypeName, HousingType HousingType, int? NumberOfHomes);
