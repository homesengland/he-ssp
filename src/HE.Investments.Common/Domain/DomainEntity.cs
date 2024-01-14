using System.Collections.ObjectModel;
using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Common.Domain;

public class DomainEntity
{
    private readonly IList<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public void Publish(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyList<IDomainEvent> GetDomainEventsAndRemove()
    {
        var toReturn = new ReadOnlyCollection<IDomainEvent>(_domainEvents.ToArray());
        _domainEvents.Clear();
        return toReturn;
    }
}
