using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFilesQuery(
    LoanApplicationId LoanApplicationId) : IRequest<TableResult<FileTableRow>>;
