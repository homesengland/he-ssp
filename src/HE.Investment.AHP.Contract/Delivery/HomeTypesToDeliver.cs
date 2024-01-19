using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Contract.Delivery;

public record HomeTypesToDeliver(HomeTypeId HomeTypeId, string HomeTypeName, int? UsedHomes);
