using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Commands;

public record ProvideDirectorLoansCommand(LoanApplicationId Id, string Exists) : IRequest<OperationResult>;
