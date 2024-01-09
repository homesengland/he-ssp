using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.Account.Contract.Users;

public record UsersAndJoinRequests(string OrganisationName, PaginationResult<UserDetails> Users, IList<UserDetails> JoinRequests);
