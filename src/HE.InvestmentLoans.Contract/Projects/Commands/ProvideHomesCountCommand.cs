using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideHomesCountCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string HomesCount) : IRequest<OperationResult>;
