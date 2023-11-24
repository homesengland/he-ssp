using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureQuery(LoanApplicationId LoanApplicationId, CompanyStructureFieldsSet CompanyStructureFieldsSet) : IRequest<GetCompanyStructureQueryResponse>;

public record GetCompanyStructureQueryResponse(CompanyStructureViewModel ViewModel);
