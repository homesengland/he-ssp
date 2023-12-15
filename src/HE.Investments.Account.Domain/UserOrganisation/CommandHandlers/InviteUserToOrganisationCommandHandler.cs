using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Domain.Organisation.ValueObjects;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
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

        var organisationId = await GetSelectedOrganisationId();
        var organisationUsers = await _organisationUsersRepository.GetOrganisationUsers(organisationId, cancellationToken);

        try
        {
            organisationUsers.InviteUser(createInvitationResult.ReturnedData!);
            await _organisationUsersRepository.Save(organisationUsers, cancellationToken);
        }
        catch (DomainValidationException ex)
        {
            return ex.OperationResult;
        }

        return OperationResult.Success();
    }

    private static OperationResult<UserInvitationEntity?> CreateInvitation(InviteUserToOrganisationCommand request)
    {
        var operationResult = OperationResult.New();
        var firstName = operationResult.Aggregate(() => new FirstName(request.FirstName));
        var lastName = operationResult.Aggregate(() => new LastName(request.LastName));
        var email = operationResult.Aggregate(() => new EmailAddress(request.Email));
        var jobTitle = operationResult.Aggregate(() => new JobTitle(request.JobTitle));
        var invitation = operationResult.Aggregate(() => new UserInvitationEntity(firstName, lastName, email, jobTitle, request.NewRole));

        return new OperationResult<UserInvitationEntity?>(operationResult.Errors, invitation);
    }

    private async Task<OrganisationId> GetSelectedOrganisationId()
    {
        var myAccount = await _userContext.GetSelectedAccount();
        return new OrganisationId(myAccount.AccountId
                                  ?? throw new InvalidOperationException("Cannot send invitation because user is not assigned to any Organisation."));
    }
}
