using HE.Investments.Common.Contract;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Common;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;

public record GetSupportingDocumentsFileQuery(LoanApplicationId LoanApplicationId, FileId FileId)
    : IRequest<DownloadedFile>;
