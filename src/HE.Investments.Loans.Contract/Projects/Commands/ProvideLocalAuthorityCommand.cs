using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;

public record ProvideLocalAuthorityCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    LocalAuthorityId? LocalAuthorityId,
    string? LocalAuthorityName) : IRequest<OperationResult>;
