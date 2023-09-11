using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Commands;

public record ProvideCompanyDebenture(LoanApplicationId Id, Debenture Debenture) : IRequest;
