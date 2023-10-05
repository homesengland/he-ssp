using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureQuery(LoanApplicationId LoanApplicationId, CompanyStructureViewOption CompanyStructureViewOption) : IRequest<GetCompanyStructureQueryResponse>;

public record GetCompanyStructureQueryResponse(CompanyStructureViewModel ViewModel);
