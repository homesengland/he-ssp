using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public record SubmitApplicationCommand(LoanApplicationEntity LoanApplication) : IRequest;
