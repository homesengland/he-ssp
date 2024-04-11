using System.Collections.ObjectModel;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Common.Domain;

public abstract class DomainEntity
{
    private readonly IList<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public void Publish(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public virtual IReadOnlyList<IDomainEvent> GetDomainEventsAndRemove()
    {
        var toReturn = new ReadOnlyCollection<IDomainEvent>(_domainEvents.ToArray());
        _domainEvents.Clear();
        return toReturn;
    }
}
