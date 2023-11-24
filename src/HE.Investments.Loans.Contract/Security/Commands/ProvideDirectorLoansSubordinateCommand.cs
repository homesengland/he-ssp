using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Security.Commands;
public record ProvideDirectorLoansSubordinateCommand(LoanApplicationId Id, string CanBeSubordinated, string ReasonWhyCannotBeSubordinated) : IRequest<OperationResult>;
