using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;

public record GetSupportingDocumentsFilesQuery(LoanApplicationId LoanApplicationId) : IRequest<IList<FileModel>>;
