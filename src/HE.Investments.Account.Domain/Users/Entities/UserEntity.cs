using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Users.ValueObjects;

namespace HE.Investments.Account.Domain.Users.Entities;

public class UserEntity
{
    public UserEntity(UserId id, string? firstName, string? lastName, string? email, string? jobTitle, UserRole? role, DateTime? lastActiveAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        JobTitle = jobTitle;
        Role = role;
        LastActiveAt = lastActiveAt;
    }

    public UserId Id { get; }

    public string? FirstName { get; }

    public string? LastName { get; }

    public string? Email { get; }

    public string? JobTitle { get; }

    public UserRole? Role { get; }

    public DateTime? LastActiveAt { get; }
}
