using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public record AddAnotherProjectCommand(LoanApplicationEntity LoanApplicationEntity) : IRequest;
