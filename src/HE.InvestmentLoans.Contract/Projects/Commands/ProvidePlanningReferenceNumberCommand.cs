using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvidePlanningReferenceNumberCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Exists, string? PlanningReferenceNumber) : IRequest<OperationResult>;
