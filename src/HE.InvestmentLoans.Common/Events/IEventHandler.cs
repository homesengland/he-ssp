using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace HE.InvestmentLoans.Common.Events;

[SuppressMessage("Naming", "CA1711", Justification = "It is part of infrastructure")]
public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : DomainEvent
{
    new Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}

