using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record class ChangeProjectNameCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Name) : IRequest<OperationResult>;
