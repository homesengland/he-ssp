using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Application.Events;

public class ApplicationHasBeenCreatedEvent : DomainEvent
{
    public ApplicationHasBeenCreatedEvent(AhpApplicationId applicationId)
    {
        ApplicationId = applicationId;
    }

    public AhpApplicationId ApplicationId { get; private set; }
}
