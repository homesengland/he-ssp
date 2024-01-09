using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationHasBeenStartedEvent(Guid LoanApplicationId) : DomainEvent;
