using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public record SubmitApplicationCommand(LoanApplicationViewModel Model) : IRequest;
