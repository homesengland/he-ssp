using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record CreateProjectCommand(LoanApplicationId LoanApplicationId) : IRequest<OperationResult<ProjectId>>;
