using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideGrantFundingInformationCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ProviderName, string Amount, string Name, string Purpose) : IRequest<OperationResult>;
