using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public record GetLoanApplicationQuery(LoanApplicationId Id) : IRequest<GetLoanApplicationQueryResponse>;

public record GetLoanApplicationQueryResponse(LoanApplicationEntity LoanApplication);
