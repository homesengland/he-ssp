using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationHasBeenResubmittedEvent() : IDomainEvent;
