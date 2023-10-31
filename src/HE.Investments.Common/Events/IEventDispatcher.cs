using HE.Investments.Common.Domain;

namespace HE.Investments.Common.Events;

public interface IEventDispatcher
{
    Task Publish(DomainEntity domainEntity, CancellationToken cancellationToken);
}
