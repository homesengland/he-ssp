using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Queries;

public record GetFundingQuery(LoanApplicationId LoanApplicationId, FundingFieldsSet FundingFieldsSet) : IRequest<GetFundingQueryResponse>;

public record GetFundingQueryResponse(FundingViewModel ViewModel);
