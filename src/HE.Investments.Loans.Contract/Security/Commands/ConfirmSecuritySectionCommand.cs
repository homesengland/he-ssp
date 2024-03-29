using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Security.Commands;

public record ConfirmSecuritySectionCommand(LoanApplicationId Id, string Answer) : IRequest<OperationResult>;
