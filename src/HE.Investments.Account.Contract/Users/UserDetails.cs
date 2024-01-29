using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.Users;

public record UserDetails(UserGlobalId Id, string? FirstName, string? LastName, string? Email, string? JobTitle, UserRole? Role, DateTime? LastActiveAt);
