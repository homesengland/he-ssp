using HE.Investments.Common.Utils.Pagination;

namespace HE.Investments.Account.Contract.Users;

public record UsersAndJoinRequests(string OrganisationName, PaginationResult<UserDetails> Users, IList<UserDetails> JoinRequests);
