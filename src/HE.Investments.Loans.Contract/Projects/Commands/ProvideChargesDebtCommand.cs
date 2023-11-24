using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideChargesDebtCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ChargesDebt, string ChargesDebtInfo) : IRequest<OperationResult>;
