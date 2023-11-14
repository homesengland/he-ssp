using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;

public record ProvideLocalAuthorityCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    LocalAuthorityId? LocalAuthorityId,
    string? LocalAuthorityName) : IRequest<OperationResult>;
