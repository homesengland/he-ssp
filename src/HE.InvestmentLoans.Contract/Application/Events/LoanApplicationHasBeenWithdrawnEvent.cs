using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Events;

namespace HE.InvestmentLoans.Contract.Application.Events;

public record LoanApplicationHasBeenWithdrawnEvent(LoanApplicationId LoanApplicationId, LoanApplicationName ApplicationName) : DomainEvent;
