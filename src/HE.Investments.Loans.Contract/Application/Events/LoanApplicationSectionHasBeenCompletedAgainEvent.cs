using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application.Events;

public record LoanApplicationSectionHasBeenCompletedAgainEvent(LoanApplicationId LoanApplicationId) : IDomainEvent;
