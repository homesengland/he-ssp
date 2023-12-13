using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using HE.Investments.Organisation.Services;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.UserOrganisation.CommandHandlers;

public class RemoveLinkBetweenUserAndOrganisationCommandHandler : IRequestHandler<RemoveLinkBetweenUserAndOrganisationCommand, OperationResult>
{
    private readonly IAccountUserContext _userContext;

    private readonly IOrganizationServiceAsync2 _organizationServiceAsync;
    private readonly IContactService _contactService;

    public RemoveLinkBetweenUserAndOrganisationCommandHandler(
        IAccountUserContext userContext,
        IOrganizationServiceAsync2 organizationServiceAsync,
        IContactService contactService)
    {
        _userContext = userContext;

        _organizationServiceAsync = organizationServiceAsync;
        _contactService = contactService;
    }

    public async Task<OperationResult> Handle(RemoveLinkBetweenUserAndOrganisationCommand request, CancellationToken cancellationToken)
    {
        var account = await _userContext.GetSelectedAccount();
        if (account.AccountId == null)
        {
            throw new InvalidOperationException("Cannot find user linked with organisation.");
        }

        await _contactService.RemoveLinkBetweenContactAndOrganisation(
            _organizationServiceAsync,
            account.AccountId.Value,
            account.UserGlobalId.Value);

        await _userContext.RefreshProfileDetails();

        return OperationResult.Success();
    }
}
