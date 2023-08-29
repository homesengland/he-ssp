using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideHowManyHomesBuiltCommand(LoanApplicationId LoanApplicationId, string? HomesBuilt) : IRequest<OperationResult>;
