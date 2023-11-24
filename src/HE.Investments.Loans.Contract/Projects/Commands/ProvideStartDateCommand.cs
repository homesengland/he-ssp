using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideStartDateCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Exists, string Year, string Month, string Day) : IRequest<OperationResult>;
