using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeHasBeenRemovedEvent(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IDomainEvent;
