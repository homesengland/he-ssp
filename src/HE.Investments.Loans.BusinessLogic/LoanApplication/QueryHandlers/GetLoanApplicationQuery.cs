using HE.Investments.Loans.BusinessLogic.ViewModel;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public record GetLoanApplicationQuery(LoanApplicationId Id, bool LoadFiles = false) : IRequest<GetLoanApplicationQueryResponse>;

public record GetLoanApplicationQueryResponse(LoanApplicationViewModel LoanApplication);
