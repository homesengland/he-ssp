namespace HE.Investments.Account.Contract.Users;

public record UsersAndJoinRequests(string OrganisationName, IList<UserDetails> Users, IList<UserDetails> JoinRequests);
