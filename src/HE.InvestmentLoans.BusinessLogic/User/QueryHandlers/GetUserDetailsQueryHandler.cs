extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.User.Queries;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _service;

    public GetUserDetailsQueryHandler(ILoanUserContext loanUserContext, IOrganizationServiceAsync2 service, IContactService contactService)
    {
        _loanUserContext = loanUserContext;
        _service = service;
        _contactService = contactService;
    }

    public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();

        var contactDto = await _contactService.RetrieveUserProfile(_service, selectedAccount.UserGlobalId.ToString()) ?? throw new NotFoundException(nameof(UserDetails), selectedAccount.UserGlobalId.ToString());

        var userDetails = new UserDetails(
            contactDto.firstName,
            contactDto.lastName,
            contactDto.jobTitle,
            contactDto.email,
            contactDto.phoneNumber,
            contactDto.secondaryPhoneNumber,
            contactDto.isTermsAndConditionsAccepted);

        return new GetUserDetailsResponse(UserDetailsMapper.MapUserDetailsToViewModel(userDetails));
    }
}
