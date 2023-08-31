using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideCompanyPurposeCommand(LoanApplicationId LoanApplicationId, string? CompanyPurpose) : IRequest<OperationResult>;
