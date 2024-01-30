using HE.Investments.Account.Api.Contract;
using HE.Investments.Account.Api.Contract.User;

namespace HE.Investments.Account.WWW.Models.Users;

public record InviteUserViewModel(string OrganisationName, string? FirstName = null, string? LastName = null, string? EmailAddress = null, string? JobTitle = null, UserRole? Role = null);
