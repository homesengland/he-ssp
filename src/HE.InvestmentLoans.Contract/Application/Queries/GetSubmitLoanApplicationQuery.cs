using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Queries;
public record GetSubmitLoanApplicationQuery(LoanApplicationId LoanApplicationId) : IRequest<GetSubmitLoanApplicationQueryResponse>;

public record GetSubmitLoanApplicationQueryResponse(SubmitLoanApplication LoanApplication);
