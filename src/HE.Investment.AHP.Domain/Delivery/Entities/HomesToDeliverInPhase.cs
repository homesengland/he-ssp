using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public record HomesToDeliverInPhase(HomeTypeId HomeTypeId, int ToDeliver);
