using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationHasBeenChangedEvent(LoanApplicationId LoanApplicationId, ApplicationStatus CurrentStatus) : IDomainEvent;
