using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public record GetLoanApplicationQuery(LoanApplicationId Id) : IRequest<GetLoanApplicationQueryResponse>;

public record GetLoanApplicationQueryResponse(LoanApplicationViewModel ViewModel);
