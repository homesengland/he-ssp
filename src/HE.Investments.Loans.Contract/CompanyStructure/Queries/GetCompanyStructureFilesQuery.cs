using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFilesQuery(
    LoanApplicationId LoanApplicationId) : IRequest<LoansTableResult>;
