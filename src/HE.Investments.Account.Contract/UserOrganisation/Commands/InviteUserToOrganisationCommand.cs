using HE.Investments.Account.Contract.Users;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Commands;

public record InviteUserToOrganisationCommand(string? FirstName, string? LastName, string? Email, string? JobTitle, UserRole? NewRole) : IRequest<OperationResult>;
