using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureQuery(LoanApplicationId LoanApplicationId) : IRequest<GetCompanyStructureQueryResponse>;

public record GetCompanyStructureQueryResponse(CompanyStructureViewModel ViewModel);
