using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class InviteUserToOrganisationCommandHandler : IRequestHandler<InviteUserToOrganisationCommand, OperationResult>
{
    private readonly IOrganisationUsersRepository _organisationUsersRepository;

    private readonly IAccountUserContext _userContext;

    public InviteUserToOrganisationCommandHandler(IOrganisationUsersRepository organisationUsersRepository, IAccountUserContext userContext)
    {
        _organisationUsersRepository = organisationUsersRepository;
        _userContext = userContext;
    }

    public async Task<OperationResult> Handle(InviteUserToOrganisationCommand request, CancellationToken cancellationToken)
    {
        var createInvitationResult = CreateInvitation(request);
        if (createInvitationResult.HasValidationErrors)
        {
            return createInvitationResult;
        }

        var account = await _userContext.GetSelectedAccount();
        var organisationUsers = await _organisationUsersRepository.GetOrganisationUsers(account.SelectedOrganisationId(), cancellationToken);

        organisationUsers.InviteUser(createInvitationResult.ReturnedData!);
        await _organisationUsersRepository.Save(organisationUsers, cancellationToken);

        return OperationResult.Success();
    }

    private static OperationResult<UserInvitationEntity?> CreateInvitation(InviteUserToOrganisationCommand request)
    {
        var operationResult = OperationResult.New();
        var firstName = operationResult.Aggregate(() => new FirstName(request.FirstName));
        var lastName = operationResult.Aggregate(() => new LastName(request.LastName));
        var email = operationResult.Aggregate(() => new EmailAddress(request.EmailAddress));
        var jobTitle = operationResult.Aggregate(() => new JobTitle(request.JobTitle));
        var invitation = operationResult.Aggregate(() => new UserInvitationEntity(firstName, lastName, email, jobTitle, request.Role));

        return new OperationResult<UserInvitationEntity?>(operationResult.Errors, invitation);
    }
}
