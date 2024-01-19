using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public class HomeTypeHasBeenUpdatedEvent : DomainEvent
{
    public HomeTypeHasBeenUpdatedEvent(AhpApplicationId applicationId, HomeTypeId homeTypeId)
    {
        ApplicationId = applicationId;
        HomeTypeId = homeTypeId;
    }

    public AhpApplicationId ApplicationId { get; private set; }

    public HomeTypeId HomeTypeId { get; private set; }
}
