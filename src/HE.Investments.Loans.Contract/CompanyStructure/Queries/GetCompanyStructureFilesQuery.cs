using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Queries;

public record GetCompanyStructureFilesQuery(
    LoanApplicationId LoanApplicationId) : IRequest<IList<FileModel>>;
