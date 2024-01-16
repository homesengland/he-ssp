using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investment.AHP.Contract.Application.Events;

public record ApplicationStatusHasBeenChangedEvent(ApplicationStatus ApplicationStatus) : DomainEvent;
