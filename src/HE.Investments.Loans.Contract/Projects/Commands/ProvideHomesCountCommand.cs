using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideHomesCountCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string HomesCount) : IRequest<OperationResult>;
