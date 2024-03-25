using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record UploadSupportingDocumentsFileCommand(LoanApplicationId LoanApplicationId, FileToUpload File) : IRequest<OperationResult<UploadedFile?>>;
