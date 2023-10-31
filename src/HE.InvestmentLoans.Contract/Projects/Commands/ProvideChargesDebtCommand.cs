using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideChargesDebtCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ChargesDebt, string ChargesDebtInfo) : IRequest<OperationResult>;
