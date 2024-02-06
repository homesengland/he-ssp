using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.User.Events;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.Services;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Account.Domain.User.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly IAccountRepository _accountRepository;

    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly IMediator _mediator;

    public ProfileRepository(IAccountRepository accountRepository, IContactService contactService, IOrganizationServiceAsync2 serviceClient, IMediator mediator)
    {
        _accountRepository = accountRepository;
        _contactService = contactService;
        _serviceClient = serviceClient;
        _mediator = mediator;
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        return await _accountRepository.GetProfileDetails(userGlobalId);
    }

    public async Task SaveAsync(UserProfileDetails userProfileDetails, UserGlobalId userGlobalId, CancellationToken cancellationToken)
    {
        var contactDto = new ContactDto
        {
            firstName = userProfileDetails.FirstName?.ToString(),
            lastName = userProfileDetails.LastName?.ToString(),
            jobTitle = userProfileDetails.JobTitle?.ToString(),
            email = userProfileDetails.Email,
            phoneNumber = userProfileDetails.TelephoneNumber?.ToString(),
            secondaryPhoneNumber = userProfileDetails.SecondaryTelephoneNumber?.ToString(),
            isTermsAndConditionsAccepted = userProfileDetails.IsTermsAndConditionsAccepted,
        };

        await _contactService.UpdateUserProfile(_serviceClient, userGlobalId.ToString(), contactDto, cancellationToken);
        await _mediator.Publish(new UserProfileChangedEvent(userGlobalId), cancellationToken);
    }
}
