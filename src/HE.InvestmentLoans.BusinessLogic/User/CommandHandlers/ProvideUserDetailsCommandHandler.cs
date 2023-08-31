extern alias Org;

using HE.InvestmentLoans.Contract.User.Commands;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org::HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;

public class ProvideUserDetailsCommandHandler : IRequestHandler<ProvideUserDetailsCommand>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly IContactService _contactService;

    private readonly IOrganizationServiceAsync2 _service;

    public ProvideUserDetailsCommandHandler(ILoanUserContext loanUserContext, IOrganizationServiceAsync2 service, IContactService contactService)
    {
        _loanUserContext = loanUserContext;
        _service = service;
        _contactService = contactService;
    }

    public async Task Handle(ProvideUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();

        var contactDto = UserDetailsMapper.MapProvideUserDetailsCommandToContactDto(request, selectedAccount.UserEmail);

        await _contactService.UpdateUserProfile(_service, selectedAccount.UserGlobalId.ToString(), contactDto);

        _loanUserContext.RefreshDetails();
    }
}
