using MediatR;

namespace HE.Investments.Account.Contract.Users.Queries;

public record GetUsersAndJoinRequestsQuery : IRequest<UsersAndJoinRequests>;
