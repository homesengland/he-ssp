using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Queries;

public record GetFundingQuery(LoanApplicationId LoanApplicationId, FundingViewOption FundingViewOption) : IRequest<GetFundingQueryResponse>;

public record GetFundingQueryResponse(FundingViewModel ViewModel);
