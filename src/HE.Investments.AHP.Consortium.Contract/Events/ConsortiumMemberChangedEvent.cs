using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.AHP.Consortium.Contract.Events;

public record ConsortiumMemberChangedEvent(ConsortiumId ConsortiumId, OrganisationId OrganisationId) : IDomainEvent;
