using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record RemoveSupportingDocumentsFileCommand(LoanApplicationId LoanApplicationId, FileId FileId) : IRequest<OperationResult>;
