using System.Collections.ObjectModel;
using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Common.Domain;

public class DomainEntity
{
    private readonly IList<DomainEvent> _domainEvents = new List<DomainEvent>();

    public void Publish(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<DomainEvent> GetDomainEventsAndRemove()
    {
        var toReturn = new ReadOnlyCollection<DomainEvent>(_domainEvents.ToArray());
        _domainEvents.Clear();
        return toReturn;
    }
}
