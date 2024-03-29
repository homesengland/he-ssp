using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Commands;

public record InviteUserToOrganisationCommand(string? FirstName, string? LastName, string? EmailAddress, string? JobTitle, UserRole? Role) : IRequest<OperationResult>;
