using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.User.Commands;

public record SaveUserProfileDetailsCommand(
    string? FirstName,
    string? LastName,
    string? JobTitle,
    string? TelephoneNumber,
    string? SecondaryTelephoneNumber) : IRequest<OperationResult>;
