using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record ProvideSupportingDocumentsCommand(LoanApplicationId LoanApplicationId, List<IFormFile>? FormFiles) : IRequest<OperationResult>;
