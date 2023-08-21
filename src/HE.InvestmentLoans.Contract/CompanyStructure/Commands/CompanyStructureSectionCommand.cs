using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record CompanyStructureSectionCommand(LoanApplicationId LoanApplicationId) : IRequest;

public record UnCompleteCompanyStructureSectionCommand(LoanApplicationId LoanApplicationId) : IRequest;
