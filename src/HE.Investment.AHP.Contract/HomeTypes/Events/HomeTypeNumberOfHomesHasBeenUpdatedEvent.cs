using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public record HomeTypeNumberOfHomesHasBeenUpdatedEvent(AhpApplicationId ApplicationId) : IDomainEvent;
