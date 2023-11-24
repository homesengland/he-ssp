using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Funding.Queries;

public record GetFundingQuery(LoanApplicationId LoanApplicationId, FundingFieldsSet FundingFieldsSet) : IRequest<GetFundingQueryResponse>;

public record GetFundingQueryResponse(FundingViewModel ViewModel);
