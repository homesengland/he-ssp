using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideStartDateCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Exists, string Year, string Month, string Day) : IRequest<OperationResult>;
