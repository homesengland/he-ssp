using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

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
                .AddValidationError("Role", ValidationErrorMessage.MustBeSelected(nameof(role)))
                .CheckErrors();
        }

        if (role!.IsNotIn(UserRole.Enhanced, UserRole.Input, UserRole.ViewOnly))
        {
            OperationResult.New()
                .AddValidationError("Role", $"Role {role} is not allowed for invitations.")
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
