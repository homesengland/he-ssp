using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideGrantFundingInformationCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string ProviderName, string Amount, string Name, string Purpose) : IRequest<OperationResult>;
