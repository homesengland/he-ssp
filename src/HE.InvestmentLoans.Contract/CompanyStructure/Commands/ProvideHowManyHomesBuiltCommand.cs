using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.InvestmentLoans.Contract.Domain;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Commands;

public record ProvideHowManyHomesBuiltCommand(LoanApplicationId LoanApplicationId, Providable<HomesBuilt> HomesBuilt) : IRequest;
