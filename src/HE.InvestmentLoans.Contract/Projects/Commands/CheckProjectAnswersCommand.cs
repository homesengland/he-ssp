using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record CheckProjectAnswersCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string YesNoAnswer) : IRequest<OperationResult>;
