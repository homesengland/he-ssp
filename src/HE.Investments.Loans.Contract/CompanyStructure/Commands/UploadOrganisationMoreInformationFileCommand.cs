using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;

namespace HE.Investments.Loans.Contract.CompanyStructure.Commands;

public record UploadOrganisationMoreInformationFileCommand(LoanApplicationId LoanApplicationId, FileToUpload File) : IRequest<OperationResult<UploadedFile?>>;
