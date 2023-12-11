using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Users.Commands;

public record ChangeUserRoleCommand(string UserId, UserRole NewRole) : IRequest<OperationResult>, IUpdateUserCommand;
