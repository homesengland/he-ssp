using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.Contract.Application.Events;

public record LoanApplicationHasBeenResubmittedEvent() : DomainEvent;
