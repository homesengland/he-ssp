using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public record AddAnotherProjectCommand(LoanApplicationId LoanApplicationId) : IRequest;
