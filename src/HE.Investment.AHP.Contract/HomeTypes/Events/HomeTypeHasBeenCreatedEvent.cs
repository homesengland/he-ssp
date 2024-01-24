using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenCreatedEvent(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, string HomeTypeName) : IDomainEvent;
