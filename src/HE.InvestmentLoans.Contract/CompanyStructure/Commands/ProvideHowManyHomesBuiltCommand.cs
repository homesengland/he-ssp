using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideHowManyHomesBuiltCommand(LoanApplicationId LoanApplicationId, HomesBuilt HomesBuilt) : IRequest;
