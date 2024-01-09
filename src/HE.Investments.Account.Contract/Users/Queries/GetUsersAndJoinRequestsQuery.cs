using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.Account.Contract.Users.Queries;

public record GetUsersAndJoinRequestsQuery(PaginationRequest UsersPaging) : IRequest<UsersAndJoinRequests>;
