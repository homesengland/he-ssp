using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideAffordableHomesCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string AffordableHomes) : IRequest<OperationResult>;
