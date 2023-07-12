using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Application.CommandHandlers;

public record SubmitApplicationCommand(LoanApplicationViewModel Model) : IRequest;
