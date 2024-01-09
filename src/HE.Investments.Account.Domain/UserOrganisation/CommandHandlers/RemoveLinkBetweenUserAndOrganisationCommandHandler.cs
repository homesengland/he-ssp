using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.UserOrganisation.Events;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Organisation.Services;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class RemoveLinkBetweenUserAndOrganisationCommandHandler : IRequestHandler<RemoveLinkBetweenUserAndOrganisationCommand, OperationResult>
{
    private readonly IAccountUserContext _userContext;

    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;
    private readonly IEventDispatcher _eventDispatcher;

    public RemoveLinkBetweenUserAndOrganisationCommandHandler(
        IAccountUserContext userContext,
        IOrganizationServiceAsync2 organizationServiceAsync,
        IContactService contactService,
        IEventDispatcher eventDispatcher)
    {
        _userContext = userContext;

        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<OperationResult> Handle(RemoveLinkBetweenUserAndOrganisationCommand request, CancellationToken cancellationToken)
    {
        var account = await _userContext.GetSelectedAccount();
        if (account.OrganisationId == null)
        {
            throw new InvalidOperationException("Cannot find user linked with organisation.");
        }

        var user = await _contactService.RetrieveUserProfile(_organizationServiceAsync, request.UserId);
        if (user == null)
        {
            throw new InvalidOperationException($"Cannot find user for given id {request.UserId}.");
        }

        await _contactService.RemoveLinkBetweenContactAndOrganisation(
            _organizationServiceAsync,
            account.OrganisationId.Value,
            request.UserId);

        await _eventDispatcher.Publish(new UserUnlinkedEvent(request.UserId, user.firstName, user.lastName), cancellationToken);

        return OperationResult.Success();
    }
}
