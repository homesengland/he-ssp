using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.InvestmentLoans.Contract.Application.Events;

public record LoanApplicationSectionHasBeenCompletedAgainEvent(LoanApplicationId LoanApplicationId) : DomainEvent;
