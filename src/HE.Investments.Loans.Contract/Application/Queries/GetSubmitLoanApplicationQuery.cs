using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;
public record GetSubmitLoanApplicationQuery(LoanApplicationId LoanApplicationId) : IRequest<GetSubmitLoanApplicationQueryResponse>;

public record GetSubmitLoanApplicationQueryResponse(SubmitLoanApplication LoanApplication);
