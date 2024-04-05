using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Scheme.Events;

public record SchemeFundingHasBeenChangedEvent(AhpApplicationId ApplicationId) : IDomainEvent;
