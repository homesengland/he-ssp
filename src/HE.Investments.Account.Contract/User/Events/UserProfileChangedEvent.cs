using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Account.Contract.User.Events;

public record UserProfileChangedEvent(string UserGlobalId) : DomainEvent;
