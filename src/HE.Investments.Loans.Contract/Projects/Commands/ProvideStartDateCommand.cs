using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideStartDateCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Exists, DateDetails? StartDate) : IRequest<OperationResult>;
