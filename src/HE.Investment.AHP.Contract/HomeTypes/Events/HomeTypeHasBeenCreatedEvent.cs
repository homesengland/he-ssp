using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.HomeTypes.Events;

public class HomeTypeHasBeenCreatedEvent : DomainEvent
{
    public HomeTypeHasBeenCreatedEvent(AhpApplicationId applicationId, HomeTypeId homeTypeId, string homeTypeName)
    {
        ApplicationId = applicationId;
        HomeTypeId = homeTypeId;
        HomeTypeName = homeTypeName;
    }

    public AhpApplicationId ApplicationId { get; private set; }

    public HomeTypeId HomeTypeId { get; private set; }

    public string HomeTypeName { get; private set; }
}
