using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public record GetLoanApplicationQuery(LoanApplicationId Id, bool LoadFiles = false) : IRequest<GetLoanApplicationQueryResponse>;

public record GetLoanApplicationQueryResponse(LoanApplicationViewModel LoanApplication);
