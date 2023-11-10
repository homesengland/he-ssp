using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Commands;
public record ProvideDirectorLoansSubordinateCommand(LoanApplicationId Id, string CanBeSubordinated, string ReasonWhyCannotBeSubordinated) : IRequest<OperationResult>;
