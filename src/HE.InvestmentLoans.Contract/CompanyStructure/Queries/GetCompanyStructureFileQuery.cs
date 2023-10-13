using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.DocumentService.Models.File;
using MediatR;

namespace HE.InvestmentLoans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFileQuery(
    Guid LoanApplicationId,
    string FileName) : IRequest<FileData>;
