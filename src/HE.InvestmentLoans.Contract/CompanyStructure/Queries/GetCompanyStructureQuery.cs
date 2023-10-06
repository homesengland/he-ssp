using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureQuery(LoanApplicationId LoanApplicationId, CompanyStructureFieldsSet CompanyStructureFieldsSet) : IRequest<GetCompanyStructureQueryResponse>;

public record GetCompanyStructureQueryResponse(CompanyStructureViewModel ViewModel);
