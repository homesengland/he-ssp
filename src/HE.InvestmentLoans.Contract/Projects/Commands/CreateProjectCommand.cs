using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record CreateProjectCommand(LoanApplicationId LoanApplicationId) : IRequest<OperationResult<ProjectId>>;
