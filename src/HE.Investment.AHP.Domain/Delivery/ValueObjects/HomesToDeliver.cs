using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public record HomesToDeliver(HomeTypeId HomeTypeId, HomeTypeName HomeTypeName, int TotalHomes);
