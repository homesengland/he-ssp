using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideCompanyPurposeCommand(LoanApplicationId LoanApplicationId, Providable<CompanyPurpose> CompanyPurpose) : IRequest;
