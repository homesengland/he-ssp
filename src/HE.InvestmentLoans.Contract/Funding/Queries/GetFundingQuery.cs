using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Queries;

public record GetFundingQuery(LoanApplicationId LoanApplicationId) : IRequest<GetFundingQueryResponse>;

public record GetFundingQueryResponse(FundingViewModel ViewModel);
