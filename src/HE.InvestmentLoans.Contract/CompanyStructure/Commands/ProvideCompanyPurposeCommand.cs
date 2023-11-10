using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideCompanyPurposeCommand(LoanApplicationId LoanApplicationId, string? CompanyPurpose) : IRequest<OperationResult>;
