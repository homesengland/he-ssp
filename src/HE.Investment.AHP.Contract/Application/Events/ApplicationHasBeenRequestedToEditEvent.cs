using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Application.Events;

public record ApplicationHasBeenRequestedToEditEvent(AhpApplicationId ApplicationId) : IDomainEvent;
