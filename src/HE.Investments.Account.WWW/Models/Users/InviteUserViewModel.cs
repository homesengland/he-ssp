using HE.Investments.Account.Contract.Users;

namespace HE.Investments.Account.WWW.Models.Users;

public record InviteUserViewModel(string OrganisationName, string FirstName = null, string LastName = null, string EmailAddress = null, string JobTitle = null, UserRole? Role = null);
