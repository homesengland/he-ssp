using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Models.Table;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFilesQuery(
    Guid LoanApplicationId) : IRequest<TableResult<FileTableRow>>;
