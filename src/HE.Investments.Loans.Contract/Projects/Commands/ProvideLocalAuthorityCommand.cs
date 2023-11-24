using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Projects.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;

public record ProvideLocalAuthorityCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    LocalAuthorityId? LocalAuthorityId,
    string? LocalAuthorityName) : IRequest<OperationResult>;
