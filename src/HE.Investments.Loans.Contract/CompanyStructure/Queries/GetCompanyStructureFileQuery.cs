using HE.Investments.DocumentService.Models;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFileQuery(
    Guid LoanApplicationId,
    string FolderPath,
    string FileName) : IRequest<DownloadFileData>;
