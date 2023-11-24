using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationHasBeenStartedEvent(Guid LoanApplicationId) : DomainEvent;
