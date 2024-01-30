using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.Domain.Users.Entities;

public class UserEntity
{
    private readonly ModificationTracker _roleModificationTracker = new();

    public UserEntity(UserGlobalId id, string? firstName, string? lastName, string? email, string? jobTitle, UserRole? role, DateTime? lastActiveAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        JobTitle = jobTitle;
        Role = role;
        LastActiveAt = lastActiveAt;
    }

    public UserGlobalId Id { get; }

    public string? FirstName { get; }

    public string? LastName { get; }

    public string? Email { get; }

    public string? JobTitle { get; }

    public UserRole? Role { get; private set; }

    public DateTime? LastActiveAt { get; }

    public bool IsRoleModified => _roleModificationTracker.IsModified;

    public void ChangeRole(UserRole newRole)
    {
        if (newRole is UserRole.Admin or UserRole.Limited or UserRole.Undefined)
        {
            var operationResult = OperationResult.New().AddValidationError("Role", "Cannot assign role to user.");
            throw new DomainValidationException(operationResult);
        }

        Role = _roleModificationTracker.Change(Role, newRole);
    }
}
