using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Users.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.Users.Entities;

public class UserEntity
{
    private readonly ModificationTracker _roleModificationTracker = new();

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

    public UserRole? Role { get; private set; }

    public DateTime? LastActiveAt { get; }

    public bool IsRoleModified => _roleModificationTracker.IsModified;

    public void ChangeRole(UserRole newRole)
    {
        Role = _roleModificationTracker.Change(Role, newRole);
    }
}
