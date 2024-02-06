using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Users.Commands;

public record ChangeUserRoleCommand(UserGlobalId UserId, UserRole NewRole) : IRequest<OperationResult>, IUpdateUserCommand;
