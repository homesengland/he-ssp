using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Account.Contract.UserOrganisation.Events;

public record UserUnlinkedEvent(UserGlobalId UserGlobalId, string FirstName, string LastName) : DomainEvent;
