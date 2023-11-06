using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideProjectTypeCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ProjectType) : IRequest<OperationResult>;
