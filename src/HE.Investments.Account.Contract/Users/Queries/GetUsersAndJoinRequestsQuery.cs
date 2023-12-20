using HE.Investments.Common.Utils.Pagination;
using MediatR;

namespace HE.Investments.Account.Contract.Users.Queries;

public record GetUsersAndJoinRequestsQuery(PaginationRequest UsersPaging) : IRequest<UsersAndJoinRequests>;
