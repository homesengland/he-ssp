using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;

public record ConfirmLocalAuthorityCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, LocalAuthority LocalAuthority) : IRequest<OperationResult>;
