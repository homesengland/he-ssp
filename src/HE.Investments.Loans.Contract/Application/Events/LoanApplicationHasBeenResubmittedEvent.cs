using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationHasBeenResubmittedEvent(LoanApplicationId ApplicationId) : IDomainEvent;
