using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public record HomesToDeliverInPhase(HomeTypeId HomeTypeId, int ToDeliver);
