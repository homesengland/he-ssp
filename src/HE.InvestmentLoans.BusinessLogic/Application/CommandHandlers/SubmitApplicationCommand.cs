using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application;

public record SubmitApplicationCommand(LoanApplicationViewModel Model) : IRequest;
