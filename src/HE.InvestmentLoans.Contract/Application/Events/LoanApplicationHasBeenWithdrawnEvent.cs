using HE.InvestmentLoans.Common.Events;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.Contract.Application.Events;

public record LoanApplicationHasBeenWithdrawnEvent(LoanApplicationId LoanApplicationId, LoanApplicationName ApplicationName) : DomainEvent;
