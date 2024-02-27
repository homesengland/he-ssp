using HE.Investments.Common.Contract;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Common;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFileQuery(LoanApplicationId LoanApplicationId, FileId FileId)
    : IRequest<DownloadedFile>;
