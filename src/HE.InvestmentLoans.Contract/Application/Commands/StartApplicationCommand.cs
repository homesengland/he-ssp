using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Commands;

public record StartApplicationCommand : IRequest<LoanApplicationId>;
