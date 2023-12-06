namespace HE.Investments.Account.Contract.Users;

public record UserDetails(string Id, string? FirstName, string? LastName, string? Email, string? JobTitle, string? Role, DateTime? LastActiveAt);
