using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideChargesDebtCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ChargesDebt, string ChargesDebtInfo) : IRequest<OperationResult>;
