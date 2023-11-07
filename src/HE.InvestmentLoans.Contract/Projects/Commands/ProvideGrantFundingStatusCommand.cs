using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideGrantFundingStatusCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Status) : IRequest<OperationResult>;
