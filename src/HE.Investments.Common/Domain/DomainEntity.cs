using System.Collections.ObjectModel;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Common.Domain;

public abstract class DomainEntity
{
    private readonly IList<IDomainEvent> _domainEvents = [];

    public void Publish(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public virtual IReadOnlyList<IDomainEvent> GetDomainEventsAndRemove()
    {
        var toReturn = new ReadOnlyCollection<IDomainEvent>([.. _domainEvents]);
        _domainEvents.Clear();
        return toReturn;
    }
}
