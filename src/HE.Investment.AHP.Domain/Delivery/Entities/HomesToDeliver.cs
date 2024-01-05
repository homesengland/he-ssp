using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public record HomesToDeliver(HomeTypeId HomeTypeId, HomeTypeName HomeTypeName, int TotalHomes);
