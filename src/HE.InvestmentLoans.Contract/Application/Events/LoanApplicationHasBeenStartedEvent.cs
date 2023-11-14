using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.Contract.Application.Events;

public record LoanApplicationHasBeenStartedEvent(Guid LoanApplicationId) : DomainEvent;
