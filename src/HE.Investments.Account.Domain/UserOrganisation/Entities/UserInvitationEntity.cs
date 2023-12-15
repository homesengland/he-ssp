using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.UserOrganisation.Entities;

public class UserInvitationEntity
{
    public UserInvitationEntity(FirstName firstName, LastName lastName, EmailAddress email, JobTitle jobTitle, UserRole? role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        JobTitle = jobTitle;

        if (role is null or UserRole.Undefined)
        {
            OperationResult.New()
                .AddValidationError(nameof(role), ValidationErrorMessage.MissingRequiredField(nameof(role)))
                .CheckErrors();
        }

        if (role!.IsNotIn(UserRole.Enhanced, UserRole.Input, UserRole.ViewOnly))
        {
            OperationResult.New()
                .AddValidationError(nameof(role), $"Role {role} is not allowed for invitations.")
                .CheckErrors();
        }

        Role = role!.Value;
    }

    public FirstName FirstName { get; }

    public LastName LastName { get; }

    public EmailAddress Email { get; }

    public JobTitle JobTitle { get; }

    public UserRole Role { get; }
}
